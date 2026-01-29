using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.DeleteExpense;

public interface IDeleteExpenseService
{
  Task<ServiceResult> DeleteExpenseAsync(
    Guid expenseId,
    string userId,
    string tenantId,
    CancellationToken cancellationToken);
}
