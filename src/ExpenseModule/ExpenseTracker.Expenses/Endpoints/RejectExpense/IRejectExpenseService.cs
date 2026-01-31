using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.RejectExpense;

public interface IRejectExpenseService
{
  Task<ServiceResult<RejectExpenseResponse>> RejectExpenseAsync(
    Guid expenseId,
    string rejectionReason,
    string userId,
    string tenantId,
    CancellationToken cancellationToken);
}
