using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.RejectExpense;

public class RejectExpenseService(IRejectExpenseRepository rejectExpenseRepository) : IRejectExpenseService
{
  public async Task<ServiceResult<RejectExpenseResponse>> RejectExpenseAsync(
    Guid expenseId,
    string rejectionReason,
    string userId,
    string tenantId,
    CancellationToken cancellationToken)
  {
    // Validate rejection reason
    if (string.IsNullOrWhiteSpace(rejectionReason))
    {
      return new ServiceResult<RejectExpenseResponse>("Rejection reason is required");
    }

    if (rejectionReason.Length > 1000)
    {
      return new ServiceResult<RejectExpenseResponse>("Rejection reason cannot exceed 1000 characters");
    }

    var userGuid = Guid.Parse(userId);
    var tenantGuid = Guid.Parse(tenantId);

    // Get the expense
    var expense = await rejectExpenseRepository.GetExpenseByIdAsync(expenseId, tenantGuid, cancellationToken);
    if (expense is null)
    {
      return new ServiceResult<RejectExpenseResponse>("Expense not found");
    }

    // Validate that the expense is in Submitted status
    if (expense.Status != ExpenseStatus.Submitted)
    {
      return new ServiceResult<RejectExpenseResponse>("Only submitted expenses can be rejected");
    }

    // Validate that the current user is the designated approver
    if (expense.SubmittedToApproverId != userGuid)
    {
      return new ServiceResult<RejectExpenseResponse>("You are not authorized to reject this expense");
    }

    // Update the expense status to Rejected
    expense.Status = ExpenseStatus.Rejected;
    expense.RejectedByUserId = userGuid;
    expense.RejectedAt = DateTime.UtcNow;
    expense.RejectionReason = rejectionReason;

    await rejectExpenseRepository.UpdateExpenseAsync(expense, cancellationToken);

    // TODO: Send rejection email notification in Phase 5

    return new ServiceResult<RejectExpenseResponse>(new RejectExpenseResponse
    {
      ExpenseId = expense.Id,
      Status = expense.Status,
      RejectedAt = expense.RejectedAt.Value,
      RejectionReason = expense.RejectionReason
    });
  }
}
