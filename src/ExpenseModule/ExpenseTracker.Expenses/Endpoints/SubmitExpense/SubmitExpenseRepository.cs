using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public class SubmitExpenseRepository(ExpenseDbContext dbContext) : ISubmitExpenseRepository
{
  public async Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await dbContext.Expenses
      .FirstOrDefaultAsync(e => e.Id == expenseId && e.TenantId == tenantId, cancellationToken);
  }

  public Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken)
  {
    dbContext.Expenses.Update(expense);
    return dbContext.SaveChangesAsync(cancellationToken);
  }
}
