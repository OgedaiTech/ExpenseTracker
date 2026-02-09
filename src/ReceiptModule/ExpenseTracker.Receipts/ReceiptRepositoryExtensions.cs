using ExpenseTracker.Receipts.Endpoints.Create;
using ExpenseTracker.Receipts.Endpoints.Delete;
using ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Receipts;

public static class ReceiptRepositoryExtensions
{
  public static void AddReceiptRepositories(this IServiceCollection services)
  {
    services.AddScoped<ICreateReceiptRepository, CreateReceiptRepository>();
    services.AddScoped<IDeleteReceiptRepository, DeleteReceiptRepository>();
    services.AddScoped<IListExpenseReceiptsRepository, ListExpenseReceiptsRepository>();
  }
}
