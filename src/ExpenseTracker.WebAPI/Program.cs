using System.Reflection;
using ExpenseTracker.Expenses;
using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Receipts;
using ExpenseTracker.Receipts.Data;
using ExpenseTracker.Tenants;
using ExpenseTracker.Tenants.Data;
using FastEndpoints;

namespace ExpenseTracker.WebAPI;

public partial class Program
{
  private static void Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.Services.AddFastEndpoints();

    builder.Services.AddHealthChecks();

    builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");
    builder.AddNpgsqlDbContext<ReceiptDbContext>("ExT");
    builder.AddNpgsqlDbContext<TenantDbContext>("ExT");

    // Add Module Services and Repositories
    List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
    builder.Services.AddExpenseServices(mediatRAssemblies);
    builder.Services.AddExpenseRepositories();
    builder.Services.AddReceiptServices(mediatRAssemblies);
    builder.Services.AddReceiptRepositories();
    builder.Services.AddTenantServices();
    builder.Services.AddTenantRepositories();

    builder.Services.AddOpenApi();

    // Set Mediatr
    builder.Services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies(mediatRAssemblies.ToArray());
      cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseFastEndpoints();

    app.MapHealthChecks("/health");

    app.Run();
  }
}
public partial class Program { }
