using ExpenseTracker.Tenants.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Tenants.Tests;

public static class ContextGenerator
{
  public static TenantDbContext CreateDbContext()
  {
    var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

    return new TenantDbContext(optionsBuilder.Options);
  }
}
