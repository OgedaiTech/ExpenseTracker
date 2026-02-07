using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public class ListUsersExpensesService(IListUsersExpensesRepository repository) : IListUsersExpensesService
{
  public async Task<ServiceResult<Expense[]>> ListUsersExpensesAsync(string userId, string tenantId, CancellationToken ct)
  {
    var hasExpense = await repository.HasExpenseAsync(userId, tenantId, ct);
    if (!hasExpense)
    {
      return new ServiceResult<Expense[]>([]);
    }

    var hasAccess = await repository.VerifyUserAccessAsync(userId, tenantId, ct);
    if (!hasAccess)
    {
      return new ServiceResult<Expense[]>(ListUsersConstants.UserDoesNotHaveAccess);
    }

    var expenses =
      (await repository.GetAllAsync(userId, tenantId, ct))
      .OrderByDescending(e => e.CreatedAt)
      .ToArray();

    return new ServiceResult<Expense[]>(expenses);
  }
}
