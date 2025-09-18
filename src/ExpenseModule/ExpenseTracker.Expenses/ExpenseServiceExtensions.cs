using ExpenseTracker.Expenses.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Expenses;

public static class ExpenseServiceExtensions
{
  public static void AddExpenseServices(this IServiceCollection services)
  {
    services.AddScoped<ICreateExpenseService, CreateExpenseService>();
  }
}
