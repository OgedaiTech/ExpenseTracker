using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Contracts;
using ExpenseTracker.Expenses.Data;
using MediatR;

namespace ExpenseTracker.Expenses.Integrations;

public class ReduceReceiptAmountFromExpenseCommandHandler(ExpenseDbContext dbContext)
  : IRequestHandler<ReduceReceiptAmountFromExpenseCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(ReduceReceiptAmountFromExpenseCommand request, CancellationToken cancellationToken)
  {
    var expense = await dbContext.Expenses.FindAsync([request.ExpenseId], cancellationToken);
    if (expense is null)
    {
      return new ServiceResult("Expense not found");
    }
    expense.Amount -= request.Amount;
    await dbContext.SaveChangesAsync(cancellationToken);
    return new ServiceResult();
  }
}
