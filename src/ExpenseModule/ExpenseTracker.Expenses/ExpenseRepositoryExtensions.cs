using ExpenseTracker.Expenses.Endpoints.Create;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Expenses;

public static class ExpenseRepositoryExtensions
{
  public static void AddExpenseRepositories(this IServiceCollection services)
  {
    services.AddScoped<ICreateExpenseRepository, CreateExpenseRepository>();
  }
}
