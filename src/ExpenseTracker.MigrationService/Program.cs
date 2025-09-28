using ExpenseTracker.Expenses.Data;
using ExpenseTracker.MigrationService;
using ExpenseTracker.Receipts.Data;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
  .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");
builder.AddNpgsqlDbContext<ReceiptDbContext>("ExT");

var host = builder.Build();
host.Run();
