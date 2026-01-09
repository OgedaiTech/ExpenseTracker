using ExpenseTracker.Expenses.Endpoints.Create;
using ExpenseTracker.Expenses.Endpoints.GetExpenseById;
using ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Expenses;

public static class ExpenseRepositoryExtensions
{
  public static void AddExpenseRepositories(this IServiceCollection services)
  {
    services.AddScoped<ICreateExpenseRepository, CreateExpenseRepository>();
    services.AddScoped<IListUsersExpensesRepository, ListUsersExpensesRepository>();
    services.AddScoped<IGetExpenseByIdRepository, GetExpenseByIdRepository>();
  }
}
