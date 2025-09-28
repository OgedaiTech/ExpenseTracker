using ExpenseTracker.Receipts.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Receipts;

public static class ReceiptServiceExtensions
{
  public static void AddReceiptServices(this IServiceCollection services)
  {
    services.AddScoped<ICreateReceiptService, CreateReceiptService>();
  }
}
