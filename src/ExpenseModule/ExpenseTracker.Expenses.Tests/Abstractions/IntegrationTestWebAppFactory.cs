using ExpenseTracker.Expenses.Data;
using ExpenseTracker.WebAPI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace ExpenseTracker.Expenses.Tests.Abstractions;

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
      services.RemoveAll(typeof(DbContextOptions<ExpenseDbContext>));
      services.AddDbContextPool<ExpenseDbContext>(options =>
      {
        options.UseNpgsql(_dbContainer.GetConnectionString());
      });
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
