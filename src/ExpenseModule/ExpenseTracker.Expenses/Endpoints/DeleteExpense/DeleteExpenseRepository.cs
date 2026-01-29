using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.DeleteExpense;

public class DeleteExpenseRepository : IDeleteExpenseRepository
{
  private readonly ExpenseDbContext _dbContext;

  public DeleteExpenseRepository(ExpenseDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await _dbContext.Expenses
      .FirstOrDefaultAsync(e => e.Id == expenseId && e.TenantId == tenantId, cancellationToken);
  }

  public Task DeleteExpenseAsync(Expense expense, CancellationToken cancellationToken)
  {
    expense.DeletedAt = DateTime.UtcNow;
    _dbContext.Expenses.Update(expense);
    return _dbContext.SaveChangesAsync(cancellationToken);
  }
}
