using ExpenseTracker.Users.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Users;

public static class UsersModuleServiceExtensions
{
  public static IServiceCollection AddUserModuleServices(this IServiceCollection services)
  {
    services.AddIdentityCore<ApplicationUser>()
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<UsersDbContext>();

    return services;
  }
}
