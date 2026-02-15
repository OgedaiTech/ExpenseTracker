namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal interface IDeleteReceiptRepository
{
  Task<bool> DeleteAsync(Guid id);
  Task<Receipt?> GetReceiptByIdAsync(Guid id, CancellationToken ct);
  Task<int> SaveChangesAsync(CancellationToken ct);
}
