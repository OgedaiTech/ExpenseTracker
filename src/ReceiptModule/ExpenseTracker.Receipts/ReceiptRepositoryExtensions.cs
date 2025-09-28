using ExpenseTracker.Receipts.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Receipts;

public static class ReceiptRepositoryExtensions
{
  public static void AddReceiptRepositories(this IServiceCollection services)
  {
    services.AddScoped<ICreateReceiptRepository, CreateReceiptRepository>();
  }
}
