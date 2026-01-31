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

public static class ExpenseRepositoryExtensions
{
  public static void AddExpenseRepositories(this IServiceCollection services)
  {
    services.AddScoped<ICreateExpenseRepository, CreateExpenseRepository>();
    services.AddScoped<IListUsersExpensesRepository, ListUsersExpensesRepository>();
    services.AddScoped<IGetExpenseByIdRepository, GetExpenseByIdRepository>();
    services.AddScoped<ISubmitExpenseRepository, SubmitExpenseRepository>();
    services.AddScoped<IApproveExpenseRepository, ApproveExpenseRepository>();
    services.AddScoped<IRejectExpenseRepository, RejectExpenseRepository>();
    services.AddScoped<IListPendingApprovalsRepository, ListPendingApprovalsRepository>();
    services.AddScoped<IUpdateExpenseRepository, UpdateExpenseRepository>();
    services.AddScoped<IDeleteExpenseRepository, DeleteExpenseRepository>();
  }
}
