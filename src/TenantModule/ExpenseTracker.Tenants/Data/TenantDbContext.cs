using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Tenants.Data;

public class TenantDbContext : DbContext
{
  public DbSet<Tenant> Tenants { get; set; }

  public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.HasDefaultSchema("Tenant");
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);
  }
}
