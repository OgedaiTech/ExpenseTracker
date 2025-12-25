using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public interface IListUsersExpensesService
{
  Task<ServiceResult<List<Expense>>> ListUsersExpensesAsync(string userId, string tenantId,
    CancellationToken ct);
}
