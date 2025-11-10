using System.Reflection;
using ExpenseTracker.Receipts.Endpoints.Create;
using ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Receipts;

public static class ReceiptServiceExtensions
{
  public static void AddReceiptServices(
    this IServiceCollection services,
    List<Assembly> mediatRAssemblies)
  {
    services.AddScoped<ICreateReceiptService, CreateReceiptService>();
    services.AddScoped<IListExpenseReceiptsService, ListExpenseReceiptsService>();
    mediatRAssemblies.Add(typeof(ReceiptServiceExtensions).Assembly);
  }
}
