using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public class ListUsersExpensesService(IListUsersExpensesRepository repository) : IListUsersExpensesService
{
  public async Task<ServiceResult<List<Expense>>> ListUsersExpensesAsync(string userId, string tenantId, CancellationToken ct)
  {
    var expenses = await repository.GetAllAsync(userId, tenantId, ct);
    return new ServiceResult<List<Expense>>(expenses);
  }
}
