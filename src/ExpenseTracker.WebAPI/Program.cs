using ExpenseTracker.Expenses;
using ExpenseTracker.Expenses.Data;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddFastEndpoints();

builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");

builder.Services.AddExpenseServices();
builder.Services.AddExpenseRepositories();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

app.Run();
