using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Contracts;
using MediatR;

namespace ExpenseTracker.Receipts.UseCases;

public class AddAmountToExpenseCommandHandler(IMediator mediator) : IRequestHandler<AddAmountToExpenseCommand, ServiceResult<Guid>>
{
  public Task<ServiceResult<Guid>> Handle(AddAmountToExpenseCommand request, CancellationToken cancellationToken)
  {
    var command = new ExpenseAmountCommand(request.ExpenseId, request.Amount);
    return mediator.Send(command, cancellationToken);
  }
}
