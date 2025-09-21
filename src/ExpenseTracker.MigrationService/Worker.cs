using System.Diagnostics;
using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.MigrationService;

public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
{
  public const string ActivitySourceName = "Migrations";
  private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

    try
    {
      using var scope = serviceProvider.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<ExpenseDbContext>();

      await RunMigrationAsync(dbContext, stoppingToken);
    }
    catch (Exception ex)
    {
      activity?.AddException(ex);
      throw;
    }

    hostApplicationLifetime.StopApplication();
  }

  private static async Task RunMigrationAsync(ExpenseDbContext dbContext, CancellationToken cancellationToken)
  {
    var strategy = dbContext.Database.CreateExecutionStrategy();
    await strategy.ExecuteAsync(async () =>
    {
      // Run migration in a transaction to avoid partial migration if it fails.
      await dbContext.Database.MigrateAsync(cancellationToken);
    });
  }
}
