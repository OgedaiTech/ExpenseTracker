using ExpenseTracker.Core;
using ExpenseTracker.Email.Contracts;
using ExpenseTracker.Users.Contracts;
using MediatR;

namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

public class ApproveExpenseService(
  IApproveExpenseRepository approveExpenseRepository,
  IMediator mediator) : IApproveExpenseService
{
  public async Task<ServiceResult<ApproveExpenseResponse>> ApproveExpenseAsync(
    Guid expenseId,
    string userId,
    string tenantId,
    CancellationToken cancellationToken)
  {
    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    // Get the expense
    var expense = await approveExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult<ApproveExpenseResponse>(ApproveExpenseConstants.ExpenseNotFound);
    }

    // Validate that the expense is in Submitted status
    if (expense.Status != ExpenseStatus.Submitted)
    {
      return new ServiceResult<ApproveExpenseResponse>(ApproveExpenseConstants.OnlySubmittedExpensesCanBeApproved);
    }

    // Validate that the current user is the designated approver
    if (expense.SubmittedToApproverId != userGuid)
    {
      return new ServiceResult<ApproveExpenseResponse>(ApproveExpenseConstants.YouAreNotAuthorizedToApproveThisExpense);
    }

    // Update the expense status to Approved
    expense.Status = ExpenseStatus.Approved;
    expense.ApprovedByUserId = userGuid;
    expense.ApprovedAt = DateTime.UtcNow;

    await approveExpenseRepository.UpdateExpenseAsync(expense, cancellationToken);

    var approver = await mediator.Send(new GetUserEmailQuery(expense.ApprovedByUserId.Value), cancellationToken);
    if (approver.Success)
    {
      var userEmailQuery = new GetUserEmailQuery(expense.CreatedByUserId);
      var expenseSubmitter = await mediator.Send(userEmailQuery, cancellationToken);
      if (expenseSubmitter.Success)
      {
        var sendEmailCommand = new SendApproveExpenseResultEmailCommand(
          expense.Name,
          approver.Data!.Email,
          expenseSubmitter.Data!.Email);
        await mediator.Send(sendEmailCommand, cancellationToken);
      }
      else
      {
        return new ServiceResult<ApproveExpenseResponse>(ApproveExpenseConstants.FailedToRetrieveSubmitterEmail);
      }
    }
    else
    {
      return new ServiceResult<ApproveExpenseResponse>(ApproveExpenseConstants.FailedToRetrieveApproverEmail);
    }

    return new ServiceResult<ApproveExpenseResponse>(new ApproveExpenseResponse
    {
      ExpenseId = expense.Id,
      Status = expense.Status,
      ApprovedAt = expense.ApprovedAt.Value
    });
  }
}
