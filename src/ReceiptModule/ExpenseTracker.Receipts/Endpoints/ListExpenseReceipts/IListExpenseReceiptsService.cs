using ExpenseTracker.Core;

namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

public interface IListExpenseReceiptsService
{
  Task<ServiceResult<List<ReceiptDto>>> ListExpenseReceiptsAsync(Guid expenseId, CancellationToken ct);
}
