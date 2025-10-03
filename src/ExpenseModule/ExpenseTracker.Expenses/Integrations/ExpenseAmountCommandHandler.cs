using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Contracts;
using ExpenseTracker.Expenses.Data;
using MediatR;

namespace ExpenseTracker.Expenses.Integrations;

public class ExpenseAmountCommandHandler(ExpenseDbContext dbContext)
  : IRequestHandler<ExpenseAmountCommand, ServiceResult<Guid>>
{
  public Task<ServiceResult<Guid>> Handle(ExpenseAmountCommand request, CancellationToken cancellationToken)
  {
    var expense = dbContext.Expenses.Find(request.ExpenseId);
    if (expense is null)
    {
      return Task.FromResult(new ServiceResult<Guid>("Expense not found"));
    }
    expense.Amount += request.Amount;
    dbContext.SaveChanges();
    return Task.FromResult(new ServiceResult<Guid>(expense.Id));
  }
}
