using ExpenseTracker.Core;
using ExpenseTracker.Email.Contracts;
using ExpenseTracker.Receipts.Contracts;
using ExpenseTracker.Users.Contracts;
using MediatR;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public class SubmitExpenseService(
  ISubmitExpenseRepository submitExpenseRepository,
  IMediator mediator) : ISubmitExpenseService
{
  public async Task<ServiceResult<SubmitExpenseResponse>> SubmitExpenseAsync(
    Guid expenseId,
    Guid approverId,
    string userId,
    string tenantId,
    CancellationToken cancellationToken)
  {
    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    // Validate that the user is not submitting to themselves
    if (userGuid == approverId)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.YouCanNotSubmitExpensesToYourselfForApproval);
    }

    // Get the expense
    var expense = await submitExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.ExpenseNotFound);
    }

    // Validate that the expense belongs to the user
    if (expense.CreatedByUserId != userGuid)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.YouCanOnlySubmitYourOwnExpenses);
    }

    // Validate that the expense is in Draft or Rejected status
    if (expense.Status != ExpenseStatus.Draft && expense.Status != ExpenseStatus.Rejected)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.OnlyDraftOrRejectedExpensesCanBeSubmitted);
    }

    // Validate that the expense has at least one receipt
    var receiptCountQuery = new GetReceiptCountQuery(expenseId);
    var receiptCountResult = await mediator.Send(receiptCountQuery, cancellationToken);
    if (receiptCountResult.Success && receiptCountResult.Data == 0)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.CannotSubmitExpenseWithoutReceipts);
    }

    // Validate that the approver exists, is in the same tenant, and has Approver role
    var isUserApproverResult = await mediator.Send(new IsUserApproverQuery(approverId, tenantGuid), cancellationToken);
    if (!isUserApproverResult.Success || !isUserApproverResult.Data)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.SelectedApproverIsNotInYourOrganization);
    }

    // Update the expense status to Submitted
    expense.Status = ExpenseStatus.Submitted;
    expense.SubmittedToApproverId = approverId;
    expense.SubmittedAt = DateTime.UtcNow;

    await submitExpenseRepository.UpdateExpenseAsync(expense, cancellationToken);

    // Send notification email to the approver
    var getApproverEmailQuery = new GetUserEmailQuery(approverId);
    var approverEmailResult = await mediator.Send(getApproverEmailQuery, cancellationToken);
    if (!approverEmailResult.Success)
    {
      return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.FailedToRetrieveApproverEmail);
    }
    else
    {
      var getSubmitterEmailQuery = new GetUserEmailQuery(userGuid);
      var submitterEmailResult = await mediator.Send(getSubmitterEmailQuery, cancellationToken);

      if (!submitterEmailResult.Success)
      {
        return new ServiceResult<SubmitExpenseResponse>(SubmitExpenseConstants.FailedToRetrieveSubmitterEmail);
      }

      var sendEmailCommand = new SendSubmitExpenseToApproverEmailCommand(
        expense.Name,
        approverId,
        approverEmailResult.Data!.Email,
        submitterEmailResult.Success ? $"{submitterEmailResult.Data!.FirstName} {submitterEmailResult.Data.LastName}" : "A user");

      await mediator.Send(sendEmailCommand, cancellationToken);
    }

    return new ServiceResult<SubmitExpenseResponse>(new SubmitExpenseResponse
    {
      ExpenseId = expense.Id,
      Status = expense.Status,
      SubmittedAt = expense.SubmittedAt.Value
    });
  }
}
