using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public interface IListUsersExpensesService
{
  Task<ServiceResult<Expense[]>> ListUsersExpensesAsync(string userId, string tenantId,
    CancellationToken ct);
}
