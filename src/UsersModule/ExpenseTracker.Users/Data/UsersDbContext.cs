using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Users.Data;

public class UsersDbContext : IdentityDbContext
{
  public UsersDbContext() { }
  public UsersDbContext(DbContextOptions<UsersDbContext> options)
      : base(options)
  {
  }

  public DbSet<ApplicationUser> ApplicationUsers { get; set; }

  protected override void OnModelCreating(ModelBuilder builder)
  {
    builder.HasDefaultSchema("Users");

    builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

    base.OnModelCreating(builder);
  }
}
