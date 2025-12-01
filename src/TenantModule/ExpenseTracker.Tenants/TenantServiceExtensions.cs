using System.Reflection;
using ExpenseTracker.Tenants.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Tenants;

public static class TenantServiceExtensions
{
  public static void AddTenantServices(
    this IServiceCollection services,
    List<Assembly> mediatRAssemblies)
  {
    services.AddScoped<ICreateTenantService, CreateTenantService>();
    mediatRAssemblies.Add(typeof(TenantServiceExtensions).Assembly);
  }
}
