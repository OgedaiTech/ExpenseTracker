using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Data;

public class ReceiptDbContext : DbContext
{
  public DbSet<Receipt> Receipts { get; set; }

  public ReceiptDbContext(DbContextOptions<ReceiptDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Receipt");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReceiptDbContext).Assembly);
  }
}
