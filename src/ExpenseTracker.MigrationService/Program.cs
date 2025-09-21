using ExpenseTracker.Expenses.Data;
using ExpenseTracker.MigrationService;

var builder = Host.CreateApplicationBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddHostedService<Worker>();

builder.Services.AddOpenTelemetry()
  .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));

builder.AddNpgsqlDbContext<ExpenseDbContext>("ExT");

var host = builder.Build();
host.Run();
