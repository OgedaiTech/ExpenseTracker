using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Contracts;
using ExpenseTracker.Receipts.UseCases;
using MediatR;

namespace ExpenseTracker.Receipts.Endpoints.Create;

public class CreateReceiptService(
  ICreateReceiptRepository createReceiptRepository,
  IMediator mediator) : ICreateReceiptService
{
  public async Task<ServiceResult> CreateReceiptAsync(CreateReceiptRequest createReceiptRequest, CancellationToken ct)
  {
    // Check expense status before creating receipt
    var statusQuery = new GetExpenseStatusQuery(createReceiptRequest.ExpenseId);
    var statusResult = await mediator.Send(statusQuery, ct);
    if (!statusResult.Success)
    {
      return new ServiceResult("Expense not found");
    }

    // Prevent receipt creation for approved expenses (Status = 2 is Approved)
    if (statusResult.Data is not null && statusResult.Data.Status == 2)
    {
      return new ServiceResult("Cannot add receipts to approved expenses");
    }

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
