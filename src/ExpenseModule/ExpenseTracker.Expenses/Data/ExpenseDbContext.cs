using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Data;

public class ExpenseDbContext : DbContext
{
  public DbSet<Expense> Expenses { get; set; }
  public ExpenseDbContext(DbContextOptions<ExpenseDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Expense");
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
  }
}
