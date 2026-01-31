using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Contracts;
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
      return new ServiceResult<SubmitExpenseResponse>("You cannot submit expenses to yourself for approval");
    }

    // Get the expense
    var expense = await submitExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult<SubmitExpenseResponse>("Expense not found");
    }

    // Validate that the expense belongs to the user
    if (expense.CreatedByUserId != userGuid)
    {
      return new ServiceResult<SubmitExpenseResponse>("You can only submit your own expenses");
    }

    // Validate that the expense is in Draft or Rejected status
    if (expense.Status != ExpenseStatus.Draft && expense.Status != ExpenseStatus.Rejected)
    {
      return new ServiceResult<SubmitExpenseResponse>("Only draft or rejected expenses can be submitted for approval");
    }

    // Validate that the expense has at least one receipt
    var receiptCountQuery = new GetReceiptCountQuery(expenseId);
    var receiptCountResult = await mediator.Send(receiptCountQuery, cancellationToken);
    if (receiptCountResult.Success && receiptCountResult.Data == 0)
    {
      return new ServiceResult<SubmitExpenseResponse>("Cannot submit expense without at least one receipt");
    }

    // Validate that the approver exists, is in the same tenant, and has Approver role
    var isApprover = await submitExpenseRepository.IsUserApproverAsync(approverId, tenantGuid, cancellationToken);
    if (!isApprover)
    {
      return new ServiceResult<SubmitExpenseResponse>("The selected user is not an approver in your organization");
    }

    // Update the expense status to Submitted
    expense.Status = ExpenseStatus.Submitted;
    expense.SubmittedToApproverId = approverId;
    expense.SubmittedAt = DateTime.UtcNow;

    await submitExpenseRepository.UpdateExpenseAsync(expense, cancellationToken);

    return new ServiceResult<SubmitExpenseResponse>(new SubmitExpenseResponse
    {
      ExpenseId = expense.Id,
      Status = expense.Status,
      SubmittedAt = expense.SubmittedAt.Value
    });
  }
}
