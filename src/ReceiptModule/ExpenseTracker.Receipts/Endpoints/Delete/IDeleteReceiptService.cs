using ExpenseTracker.Core;

namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal interface IDeleteReceiptService
{
  Task<ServiceResult> DeleteReceiptAsync(Guid receiptId, CancellationToken ct);
}
