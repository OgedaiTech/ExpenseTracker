using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Tests;

public static class ContextGenerator
{
  public static ExpenseDbContext CreateDbContext()
  {
    var optionsBuilder = new DbContextOptionsBuilder<ExpenseDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

    return new ExpenseDbContext(optionsBuilder.Options);
  }
}
