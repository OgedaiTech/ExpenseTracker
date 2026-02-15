using ExpenseTracker.Receipts.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal class DeleteReceiptRepository(ReceiptDbContext dbContext) : IDeleteReceiptRepository
{
  public Task<bool> DeleteAsync(Guid id)
  {
    var entity = dbContext.Receipts.FirstOrDefault(r => r.Id == id);
    if (entity != null)
    {
      dbContext.Receipts.Remove(entity);
      return Task.FromResult(true);
    }
    return Task.FromResult(false);
  }

  public Task<Receipt?> GetReceiptByIdAsync(Guid id, CancellationToken ct)
  {
    return dbContext.Receipts.FirstOrDefaultAsync(r => r.Id == id, ct);
  }

  public async Task<int> SaveChangesAsync(CancellationToken ct)
  {
    var result = await dbContext.SaveChangesAsync(ct);
    return result;
  }
}
