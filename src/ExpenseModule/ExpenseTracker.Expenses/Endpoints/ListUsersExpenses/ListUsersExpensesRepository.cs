using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public class ListUsersExpensesRepository(ExpenseDbContext context) : IListUsersExpensesRepository
{
  public Task<List<Expense>> GetAllAsync(string userId, string tenantId, CancellationToken ct)
  {
    return context.Expenses
    .Where(
      e => e.CreatedByUserId == Guid.Parse(userId) &&
      e.TenantId == Guid.Parse(tenantId))
    .ToListAsync(ct);
  }
}
