using ExpenseTracker.Tenants.Data;
using ExpenseTracker.WebAPI;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace ExpenseTracker.Tenants.Tests.Abstractions;

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
    builder.UseEnvironment("Test");
    builder.ConfigureTestServices(services =>
    {
      services.RemoveAll<DbContextOptions<TenantDbContext>>();
      services.AddDbContextPool<TenantDbContext>(options =>
      {
        options.UseNpgsql(_dbContainer.GetConnectionString());
      });

      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = TestAuthHandler.SchemeName;
        options.DefaultChallengeScheme = TestAuthHandler.SchemeName;
      }).AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
          TestAuthHandler.SchemeName, _ => { });

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
