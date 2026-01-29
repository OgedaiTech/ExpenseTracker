using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

public class ApproveExpenseService(IApproveExpenseRepository approveExpenseRepository) : IApproveExpenseService
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
      return new ServiceResult<ApproveExpenseResponse>("Expense not found");
    }

    // Validate that the expense is in Submitted status
    if (expense.Status != ExpenseStatus.Submitted)
    {
      return new ServiceResult<ApproveExpenseResponse>("Only submitted expenses can be approved");
    }

    // Validate that the current user is the designated approver
    if (expense.SubmittedToApproverId != userGuid)
    {
      return new ServiceResult<ApproveExpenseResponse>("You are not authorized to approve this expense");
    }

    // Update the expense status to Approved
    expense.Status = ExpenseStatus.Approved;
    expense.ApprovedByUserId = userGuid;
    expense.ApprovedAt = DateTime.UtcNow;

    await approveExpenseRepository.UpdateExpenseAsync(expense, cancellationToken);

    // TODO: Send approval email notification in Phase 5

    return new ServiceResult<ApproveExpenseResponse>(new ApproveExpenseResponse
    {
      ExpenseId = expense.Id,
      Status = expense.Status,
      ApprovedAt = expense.ApprovedAt.Value
    });
  }
}
