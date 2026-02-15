using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Contracts;
using MediatR;

namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal class DeleteReceiptService(
  IDeleteReceiptRepository deleteReceiptRepository,
  IMediator mediator) : IDeleteReceiptService
{
  public async Task<ServiceResult> DeleteReceiptAsync(Guid receiptId, CancellationToken ct)
  {
    var receipt = await deleteReceiptRepository.GetReceiptByIdAsync(receiptId, ct);
    if (receipt is null)
    {
      return new ServiceResult(DeleteReceiptConstants.RECEIPT_NOT_FOUND);
    }

    var getExpenseStatusQuery = new GetExpenseStatusQuery(receipt.ExpenseId);
    var expenseStatusResult = await mediator.Send(getExpenseStatusQuery, ct);
    if (expenseStatusResult.Data is not null &&
        !expenseStatusResult.Success ||
        expenseStatusResult.Data!.Status == (int)ExpenseStatus.Approved ||
        expenseStatusResult.Data.Status == (int)ExpenseStatus.Submitted)
    {
      // approved or submitted expenses should not allow receipt deletion (immutability check)
      return new ServiceResult(DeleteReceiptConstants.DELETE_FAILED);
    }

    var isEntityStateChangedToDeleted = await deleteReceiptRepository.DeleteAsync(receiptId);
    if (!isEntityStateChangedToDeleted)
    {
      return new ServiceResult(DeleteReceiptConstants.DELETE_FAILED);
    }

    var savedChanges = await deleteReceiptRepository.SaveChangesAsync(ct);
    if (savedChanges > 0)
    {
      var reduceReceiptAmountFromExpenseCommand = new ReduceReceiptAmountFromExpenseCommand(receipt.ExpenseId, receipt.Amount);
      await mediator.Send(reduceReceiptAmountFromExpenseCommand, ct);
      return new ServiceResult();
    }
    return new ServiceResult(DeleteReceiptConstants.DELETE_FAILED);
  }
}

public enum ExpenseStatus
{
  Draft = 0,
  Submitted = 1,
  Approved = 2,
  Rejected = 3
}
