namespace ExpenseTracker.Receipts.Endpoints.Create;

public interface ICreateReceiptRepository
{
  Task CreateReceiptAsync(CreateReceiptRequest createReceiptRequest, CancellationToken ct);
}
