using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.GetExpenseById;

public class GetExpenseByIdRepository(ExpenseDbContext context) : IGetExpenseByIdRepository
{
    public Task<Expense?> GetExpenseByIdAsync(string expenseId, string userId, string tenantId, CancellationToken ct)
    {
        return context.Expenses
            .Where(e => e.Id == Guid.Parse(expenseId) &&
                        e.CreatedByUserId == Guid.Parse(userId) &&
                        e.TenantId == Guid.Parse(tenantId))
            .FirstOrDefaultAsync(ct);
    }
}
