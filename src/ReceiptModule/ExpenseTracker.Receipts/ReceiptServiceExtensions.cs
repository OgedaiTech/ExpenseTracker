using System.Reflection;
using ExpenseTracker.Receipts.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Receipts;

public static class ReceiptServiceExtensions
{
  public static void AddReceiptServices(
    this IServiceCollection services,
    List<Assembly> mediatRAssemblies)
  {
    services.AddScoped<ICreateReceiptService, CreateReceiptService>();
    mediatRAssemblies.Add(typeof(ReceiptServiceExtensions).Assembly);
  }
}
