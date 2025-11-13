namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

public interface IListExpenseReceiptsRepository
{
  Task<List<ReceiptDto>> GetReceiptsByExpenseIdAsync(Guid expenseId, CancellationToken ct);
}
