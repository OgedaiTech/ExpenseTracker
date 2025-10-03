using ExpenseTracker.Core;

namespace ExpenseTracker.Receipts.Endpoints.Create;

public interface ICreateReceiptService
{
  Task<ServiceResult> CreateReceiptAsync(CreateReceiptRequest createReceiptRequest, CancellationToken ct);
}
