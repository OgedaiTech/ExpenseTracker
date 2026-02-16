using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Accounting.Data;

public class AccountingDbContext : DbContext
{
  public DbSet<TenantAccountingSettings> TenantAccountingSettings { get; set; }

  public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Accounting");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);
  }
}
