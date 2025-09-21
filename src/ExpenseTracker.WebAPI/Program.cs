using ExpenseTracker.Expenses;
using ExpenseTracker.Expenses.Data;
using FastEndpoints;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddFastEndpoints();

builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");

builder.Services.AddExpenseServices();
builder.Services.AddExpenseRepositories();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

app.Run();
