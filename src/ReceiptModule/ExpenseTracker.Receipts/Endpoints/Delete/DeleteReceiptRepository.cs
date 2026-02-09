using ExpenseTracker.Receipts.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Endpoints.Delete;

internal class DeleteReceiptRepository(ReceiptDbContext dbContext) : IDeleteReceiptRepository
{
  public Task<bool> DeleteAsync(Guid id)
  {
    var entityEntry = dbContext.Receipts.Remove(new Receipt { Id = id });
    return Task.FromResult(entityEntry.State == EntityState.Deleted);
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
