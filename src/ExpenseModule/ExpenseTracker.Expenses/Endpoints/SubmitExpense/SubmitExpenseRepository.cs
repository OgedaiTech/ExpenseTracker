using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public class SubmitExpenseRepository : ISubmitExpenseRepository
{
  private readonly ExpenseDbContext _dbContext;
  private readonly IMediator _mediator;

  public SubmitExpenseRepository(ExpenseDbContext dbContext, IMediator mediator)
  {
    _dbContext = dbContext;
    _mediator = mediator;
  }

  public async Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await _dbContext.Expenses
      .FirstOrDefaultAsync(e => e.Id == expenseId && e.TenantId == tenantId, cancellationToken);
  }

  public async Task<bool> IsUserApproverAsync(Guid userId, Guid tenantId, CancellationToken cancellationToken)
  {
    var result = await _mediator.Send(new IsUserApproverQuery(userId, tenantId), cancellationToken);
    return result.Success && result.Data;
  }

  public Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken)
  {
    _dbContext.Expenses.Update(expense);
    return _dbContext.SaveChangesAsync(cancellationToken);
  }
}
