using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

public class ApproveExpenseRepository : IApproveExpenseRepository
{
  private readonly ExpenseDbContext _dbContext;

  public ApproveExpenseRepository(ExpenseDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await _dbContext.Expenses
      .FirstOrDefaultAsync(e => e.Id == expenseId && e.TenantId == tenantId, cancellationToken);
  }

  public Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken)
  {
    _dbContext.Expenses.Update(expense);
    return _dbContext.SaveChangesAsync(cancellationToken);
  }
}
