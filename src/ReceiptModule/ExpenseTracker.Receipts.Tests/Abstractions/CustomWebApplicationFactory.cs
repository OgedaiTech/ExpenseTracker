using ExpenseTracker.Receipts.Data;
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

namespace ExpenseTracker.Receipts.Tests.Abstractions;

public class CustomWebApplicationFactory
  : WebApplicationFactory<Program>, IAsyncLifetime
{
  private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
    .WithImage("postgres:latest")
    .WithDatabase("ExT")
    .WithUsername("postgres")
    .WithPassword("postgres")
    .Build();

  protected override void ConfigureWebHost(
    IWebHostBuilder builder)
  {
    builder.ConfigureTestServices(services =>
    {
      services.RemoveAll<DbContextOptions<ReceiptDbContext>>();
      services.AddDbContextPool<ReceiptDbContext>(options =>
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

  public Task InitializeAsync()
  {
    return _dbContainer.StartAsync();
  }

  Task IAsyncLifetime.DisposeAsync()
  {
    return _dbContainer.StopAsync();
  }
}
