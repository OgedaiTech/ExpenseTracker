using ExpenseTracker.Receipts.Data;
using ExpenseTracker.WebAPI;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace ExpenseTracker.Receipts.Tests.Abstractions;

public class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
    .WithImage("postgres:latest")
    .WithDatabase("ExT")
    .WithUsername("postgres")
    .WithPassword("postgres")
    .Build();

  protected override void ConfigureWebHost(IWebHostBuilder builder)
  {
    builder.ConfigureTestServices(services =>
    {
      services.RemoveAll(typeof(DbContextOptions<ReceiptDbContext>));
      services.AddDbContextPool<ReceiptDbContext>(options =>
      {
        options.UseNpgsql(_dbContainer.GetConnectionString());
      });

      // Mock MediatR
      services.AddSingleton<IMediator, TestMediator>();
    });
  }

  public async Task InitializeAsync()
  {
    await _dbContainer.StartAsync();
  }

  async Task IAsyncLifetime.DisposeAsync()
  {
    await _dbContainer.StopAsync();
  }
}
