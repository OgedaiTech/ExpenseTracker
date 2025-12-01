using System.Reflection;
using ExpenseTracker.Users.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Users;

public static class UsersModuleServiceExtensions
{
  public static IServiceCollection AddUserModuleServices(
    this IServiceCollection services,
    List<Assembly> mediatRAssemblies
    )
  {
    services.AddIdentityCore<ApplicationUser>()
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<UsersDbContext>();
    mediatRAssemblies.Add(typeof(UsersModuleServiceExtensions).Assembly);
    return services;
  }
}
