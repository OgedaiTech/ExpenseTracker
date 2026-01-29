using System.Reflection;
using ExpenseTracker.Expenses.Endpoints.Create;
using ExpenseTracker.Expenses.Endpoints.GetExpenseById;
using ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;
using ExpenseTracker.Expenses.Endpoints.SubmitExpense;
using ExpenseTracker.Expenses.Endpoints.ApproveExpense;
using ExpenseTracker.Expenses.Endpoints.RejectExpense;
using ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;
using ExpenseTracker.Expenses.Endpoints.UpdateExpense;
using ExpenseTracker.Expenses.Endpoints.DeleteExpense;
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
    services.AddScoped<IGetExpenseByIdService, GetExpenseByIdService>();
    services.AddScoped<ISubmitExpenseService, SubmitExpenseService>();
    services.AddScoped<IApproveExpenseService, ApproveExpenseService>();
    services.AddScoped<IRejectExpenseService, RejectExpenseService>();
    services.AddScoped<IListPendingApprovalsService, ListPendingApprovalsService>();
    services.AddScoped<IUpdateExpenseService, UpdateExpenseService>();
    services.AddScoped<IDeleteExpenseService, DeleteExpenseService>();

    // if using MediatR in this module, add any assmeblies that contain handlers to the module
    mediatRAssemblies.Add(typeof(ExpenseServiceExtensions).Assembly);
  }
}
