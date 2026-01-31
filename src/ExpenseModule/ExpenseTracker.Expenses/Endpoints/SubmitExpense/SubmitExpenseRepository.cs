using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public class SubmitExpenseRepository(ExpenseDbContext dbContext, IMediator mediator) : ISubmitExpenseRepository
{
  public async Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await dbContext.Expenses
      .FirstOrDefaultAsync(e => e.Id == expenseId && e.TenantId == tenantId, cancellationToken);
  }

  public async Task<bool> IsUserApproverAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    var result = await mediator.Send(new IsUserApproverQuery(userId, tenantId), cancellationToken);
    return result.Success && result.Data;
  }

  public Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken)
  {
    dbContext.Expenses.Update(expense);
    return dbContext.SaveChangesAsync(cancellationToken);
  }
}
