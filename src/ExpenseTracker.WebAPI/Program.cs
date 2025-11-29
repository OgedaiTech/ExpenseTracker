using System.Reflection;
using ExpenseTracker.Expenses;
using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Receipts;
using ExpenseTracker.Receipts.Data;
using ExpenseTracker.Tenants;
using ExpenseTracker.Tenants.Data;
using ExpenseTracker.Users;
using ExpenseTracker.Users.Data;
using FastEndpoints;
using FastEndpoints.Security;

namespace ExpenseTracker.WebAPI;

public partial class Program
{
  private static async Task Main(string[] args)
  {
    var builder = WebApplication.CreateBuilder(args);

    builder.AddServiceDefaults();

    builder.Services.AddFastEndpoints()
      .AddAuthenticationJwtBearer(options =>
      {
        options.SigningKey = builder.Configuration["Auth:JwtSecret"];
      })
      .AddAuthorization();

    builder.Services.AddHealthChecks();

    builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");
    builder.AddNpgsqlDbContext<ReceiptDbContext>("ExT");
    builder.AddNpgsqlDbContext<TenantDbContext>("ExT");
    builder.AddNpgsqlDbContext<UsersDbContext>("ExT");

    // Add Module Services and Repositories
    List<Assembly> mediatRAssemblies = [typeof(Program).Assembly];
    builder.Services.AddExpenseServices(mediatRAssemblies);
    builder.Services.AddExpenseRepositories();
    builder.Services.AddReceiptServices(mediatRAssemblies);
    builder.Services.AddReceiptRepositories();
    builder.Services.AddTenantServices();
    builder.Services.AddTenantRepositories();
    builder.Services.AddUserModuleServices();

    builder.Services.AddOpenApi();

    // Set Mediatr
    builder.Services.AddMediatR(cfg =>
    {
      cfg.RegisterServicesFromAssemblies([.. mediatRAssemblies]);
      cfg.LicenseKey = builder.Configuration["MediatR:LicenseKey"];
    });

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
      app.MapOpenApi();
    }

    app.UseHttpsRedirection();

    app.UseAuthentication()
       .UseAuthorization();

    app.UseFastEndpoints();

    // Seed Data
    if (!app.Environment.IsEnvironment("Test"))
    {
      using var scope = app.Services.CreateScope();
      await SeedData.InitiliazeAsync(scope.ServiceProvider, builder.Configuration);
    }

    app.MapHealthChecks("/health");

    await app.RunAsync();
  }
}
public partial class Program { }
