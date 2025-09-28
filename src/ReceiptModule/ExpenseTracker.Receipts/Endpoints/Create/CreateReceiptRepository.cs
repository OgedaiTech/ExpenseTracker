using ExpenseTracker.Receipts.Data;

namespace ExpenseTracker.Receipts.Endpoints.Create;

public class CreateReceiptRepository(ReceiptDbContext dbContext) : ICreateReceiptRepository
{
  public Task CreateReceiptAsync(CreateReceiptRequest createReceiptRequest, CancellationToken ct)
  {
    dbContext.Receipts.Add(new Receipt
    {
      Amount = createReceiptRequest.Amount,
      Vendor = createReceiptRequest.Vendor,
      ReceiptNo = createReceiptRequest.ReceiptNo,
      CreatedAt = DateTime.UtcNow,
      ExpenseId = createReceiptRequest.ExpenseId,
      Date = createReceiptRequest.Date
    });
    return dbContext.SaveChangesAsync(ct);
  }
}
