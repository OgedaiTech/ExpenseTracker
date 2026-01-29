using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.DeleteExpense;

public class DeleteExpenseRepository(ExpenseDbContext dbContext) : IDeleteExpenseRepository
{
  public async Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await dbContext.Expenses
      .FirstOrDefaultAsync(e => e.Id == expenseId && e.TenantId == tenantId, cancellationToken);
  }

  public Task DeleteExpenseAsync(Expense expense, CancellationToken cancellationToken)
  {
    expense.DeletedAt = DateTime.UtcNow;
    dbContext.Expenses.Update(expense);
    return dbContext.SaveChangesAsync(cancellationToken);
  }
}
