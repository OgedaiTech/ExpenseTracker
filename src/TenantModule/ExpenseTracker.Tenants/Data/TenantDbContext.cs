using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Tenants.Data;

public class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options)
{
  public DbSet<Tenant> Tenants { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Tenant");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);
  }
}
