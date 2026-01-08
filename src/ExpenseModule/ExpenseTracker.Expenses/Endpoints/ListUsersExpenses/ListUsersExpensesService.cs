using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public class ListUsersExpensesService(IListUsersExpensesRepository repository) : IListUsersExpensesService
{
  public async Task<ServiceResult<Expense[]>> ListUsersExpensesAsync(string userId, string tenantId, CancellationToken ct)
  {
    var hasAccess = await repository.VerifyUserAccessAsync(userId, tenantId, ct);
    if (!hasAccess)
    {
      return new ServiceResult<Expense[]>("User does not have access to the requested expenses.");
    }

    var expenses =
      (await repository.GetAllAsync(userId, tenantId, ct))
      .OrderByDescending(e => e.CreatedAt)
      .ToArray();

    return new ServiceResult<Expense[]>(expenses);
  }
}
