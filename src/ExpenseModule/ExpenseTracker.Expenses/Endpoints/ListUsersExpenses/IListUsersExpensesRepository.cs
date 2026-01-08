namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public interface IListUsersExpensesRepository
{
  Task<List<Expense>> GetAllAsync(string userId, string tenantId, CancellationToken ct);
  Task<bool> VerifyUserAccessAsync(string requestingUserId, string tenantId, CancellationToken ct);
}
