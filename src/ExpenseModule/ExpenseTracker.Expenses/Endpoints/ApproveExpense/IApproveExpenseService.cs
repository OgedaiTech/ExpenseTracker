using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

public interface IApproveExpenseService
{
  Task<ServiceResult<ApproveExpenseResponse>> ApproveExpenseAsync(
    Guid expenseId,
    string userId,
    string tenantId,
    CancellationToken cancellationToken);
}
