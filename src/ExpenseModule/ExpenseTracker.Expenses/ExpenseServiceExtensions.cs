using System.Reflection;
using ExpenseTracker.Expenses.Endpoints.Create;
using ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Expenses;

public static class ExpenseServiceExtensions
{
  public static void AddExpenseServices(
    this IServiceCollection services,
    List<Assembly> mediatRAssemblies)
  {
    services.AddScoped<ICreateExpenseService, CreateExpenseService>();
    services.AddScoped<IListUsersExpensesService, ListUsersExpensesService>();

    // if using MediatR in this module, add any assmeblies that contain handlers to the module
    mediatRAssemblies.Add(typeof(ExpenseServiceExtensions).Assembly);
  }
}
