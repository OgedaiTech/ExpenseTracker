using ExpenseTracker.Tenants.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Tenants;

public static class TenantServiceExtensions
{
  public static void AddTenantServices(
    this IServiceCollection services)
  {
    services.AddScoped<ICreateTenantService, CreateTenantService>();
  }
}
