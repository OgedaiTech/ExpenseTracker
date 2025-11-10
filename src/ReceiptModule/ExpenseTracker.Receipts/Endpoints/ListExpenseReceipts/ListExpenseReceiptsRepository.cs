using ExpenseTracker.Receipts.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

public class ListExpenseReceiptsRepository
  (ReceiptDbContext receiptDbContext) : IListExpenseReceiptsRepository
{
  public async Task<List<ReceiptDto>> GetReceiptsByExpenseIdAsync(Guid expenseId, CancellationToken ct)
  {
    var receipts = await receiptDbContext.Receipts
      .Where(r => r.ExpenseId == expenseId)
      .Select(r => new ReceiptDto
      {
        Id = r.Id,
        ReceiptNo = r.ReceiptNo,
        Date = r.Date,
        Amount = r.Amount,
        Vendor = r.Vendor,
        CreatedAt = r.CreatedAt
      })
      .ToListAsync(ct);

    return receipts;
  }
}
