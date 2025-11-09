using System;
using ExpenseTracker.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Users.Tests;

public static class ContextGenerator
{
  public static UsersDbContext CreateDbContext()
  {
    var optionsBuilder = new DbContextOptionsBuilder<UsersDbContext>()
      .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());

    return new UsersDbContext(optionsBuilder.Options);
  }
}
