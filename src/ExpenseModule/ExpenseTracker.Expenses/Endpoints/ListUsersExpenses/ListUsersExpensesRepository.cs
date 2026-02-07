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
      e.TenantId == Guid.Parse(tenantId) &&
      e.DeletedAt == null)
    .ToListAsync(ct);
  }

  public Task<bool> HasExpenseAsync(string userId, string tenantId, CancellationToken ct)
  {
    return context.Expenses.AnyAsync(e =>
      e.CreatedByUserId == Guid.Parse(userId) &&
      e.TenantId == Guid.Parse(tenantId) &&
      e.DeletedAt == null, ct);
  }

  public Task<bool> VerifyUserAccessAsync(string requestingUserId, string tenantId, CancellationToken ct)
  {
    return context.Expenses
      .AnyAsync(
        u => u.CreatedByUserId == Guid.Parse(requestingUserId) &&
        u.TenantId == Guid.Parse(tenantId), ct);
  }
}
