using ExpenseTracker.Receipts.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ExpenseTracker.Receipts.Tests.Abstractions;

public class BaseIntegrationTest : IClassFixture<IntegrationTestWebAppFactory>, IDisposable
{
  private readonly IServiceScope _scope;
  protected ReceiptDbContext DbContext;
  protected HttpClient Client;
  private bool _disposed = false;

  protected BaseIntegrationTest(IntegrationTestWebAppFactory factory)
  {
    _scope = factory.Services.CreateScope();
    Client = factory.CreateClient();
    DbContext = _scope.ServiceProvider.GetRequiredService<ReceiptDbContext>();
  }

  protected virtual void Dispose(bool disposing)
  {
    if (!_disposed)
    {
      if (disposing)
      {
        // Dispose managed resources
        _scope.Dispose();
      }

      // Dispose unmanaged resources

      _disposed = true;
    }
  }

  protected async Task ResetDatabaseAsync()
  {
    await DbContext.Database.EnsureDeletedAsync();
    await DbContext.Database.MigrateAsync();
  }

  public void Dispose()
  {
    Dispose(true);
    GC.SuppressFinalize(this);
  }

  ~BaseIntegrationTest()
  {
    Dispose(false);
  }
}
