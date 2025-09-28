using ExpenseTracker.Expenses;
using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Receipts;
using ExpenseTracker.Receipts.Data;
using FastEndpoints;

namespace ExpenseTracker.WebAPI;

public partial class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.Services.AddFastEndpoints();

    builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");
    builder.AddNpgsqlDbContext<ReceiptDbContext>("ExT");

    builder.Services.AddExpenseServices();
    builder.Services.AddExpenseRepositories();
    builder.Services.AddReceiptServices();
    builder.Services.AddReceiptRepositories();

    builder.Services.AddOpenApi();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseFastEndpoints();

    app.Run();
  }
}
public partial class Program { }
