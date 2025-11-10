using ExpenseTracker.Core;

namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

public class ListExpenseReceiptsService
  (IListExpenseReceiptsRepository repository) : IListExpenseReceiptsService
{
  public async Task<ServiceResult<List<ReceiptDto>>> ListExpenseReceiptsAsync(Guid expenseId, CancellationToken ct)
  {
    var receipts = await repository.GetReceiptsByExpenseIdAsync(expenseId, ct);
    return new ServiceResult<List<ReceiptDto>>(receipts);
  }
}
