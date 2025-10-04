using ExpenseTracker.Receipts.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Tests;

public static class ContextGenerator
{
  public static ReceiptDbContext CreateDbContext()
  {
    var optionsBuilder = new DbContextOptionsBuilder<ReceiptDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

    return new ReceiptDbContext(optionsBuilder.Options);
  }
}
