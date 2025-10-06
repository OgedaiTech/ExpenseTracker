using ExpenseTracker.Tenants.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Tenants;

public static class TenantRepositoryExtensions
{
  public static void AddTenantRepositories(
    this IServiceCollection services)
  {
    services.AddScoped<ICreateTenantRepository, CreateTenantRepository>();
  }
}
