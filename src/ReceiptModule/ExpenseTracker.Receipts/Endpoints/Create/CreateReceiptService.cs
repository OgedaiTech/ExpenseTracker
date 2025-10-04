using ExpenseTracker.Core;
using ExpenseTracker.Receipts.UseCases;
using MediatR;

namespace ExpenseTracker.Receipts.Endpoints.Create;

public class CreateReceiptService(
  ICreateReceiptRepository createReceiptRepository,
  IMediator mediator) : ICreateReceiptService
{
  public async Task<ServiceResult> CreateReceiptAsync(CreateReceiptRequest createReceiptRequest, CancellationToken ct)
  {
    await createReceiptRepository.CreateReceiptAsync(createReceiptRequest, ct);

    // Add the receipt amount to the expense by making a call to ExpenseModule
    var command = new AddAmountToExpenseCommand(createReceiptRequest.ExpenseId, createReceiptRequest.Amount);
    var increaseAmountResult = await mediator.Send(command, ct);
    if (!increaseAmountResult.Success)
    {
      return new ServiceResult(ServiceConstants.CANT_ADD_AMOUNT_TO_EXPENSE);
    }

    return new ServiceResult();
  }
}
