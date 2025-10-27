using ExpenseTracker.Users.Data;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Users;

public static class UsersModuleServiceExtensions
{
  public static IServiceCollection AddUserModuleServices(this IServiceCollection services)
  {
    services.AddIdentityCore<ApplicationUser>()
      .AddEntityFrameworkStores<UsersDbContext>();

    return services;
  }
}
