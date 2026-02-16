using System.Reflection;
using ExpenseTracker.Accounting.Endpoints.ConfigureAccountingSettings;
using ExpenseTracker.Accounting.Providers.Parasut;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Accounting;

public static class AccountingServiceExtensions
{
  public static IServiceCollection AddAccountingServices(
      this IServiceCollection services,
      List<Assembly> mediatRAssemblies,
      IConfiguration configuration)
  {
    services.Configure<ParasutGlobalSettings>(configuration.GetSection("Accounting:Parasut"));

    services.AddHttpClient("Parasut");

    services.AddSingleton<ParasutTokenCache>();
    services.AddScoped<IAccountingProvider, ParasutAccountingProvider>();

    services.AddScoped<IConfigureAccountingSettingsService, ConfigureAccountingSettingsService>();

    mediatRAssemblies.Add(typeof(AccountingServiceExtensions).Assembly);

    return services;
  }
}
