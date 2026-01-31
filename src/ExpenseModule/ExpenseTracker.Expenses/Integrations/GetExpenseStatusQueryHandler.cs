using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Contracts;
using ExpenseTracker.Expenses.Data;
using MediatR;

namespace ExpenseTracker.Expenses.Integrations;

public class GetExpenseStatusQueryHandler(ExpenseDbContext dbContext)
  : IRequestHandler<GetExpenseStatusQuery, ServiceResult<ExpenseStatusDto>>
{
  public Task<ServiceResult<ExpenseStatusDto>> Handle(GetExpenseStatusQuery request, CancellationToken cancellationToken)
  {
    var expense = dbContext.Expenses.Find(request.ExpenseId);
    if (expense is null)
    {
      return Task.FromResult(new ServiceResult<ExpenseStatusDto>("Expense not found"));
    }

    return Task.FromResult(new ServiceResult<ExpenseStatusDto>(new ExpenseStatusDto((int)expense.Status)));
  }
}
