using System.Reflection;
using ExpenseTracker.Users.CsvService;
using ExpenseTracker.Users.Data;
using ExpenseTracker.Users.EmailService;
using ExpenseTracker.Users.TokenService;
using ExpenseTracker.Users.UserEndpoints.BulkCreate;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.Users;

public static class UsersModuleServiceExtensions
{
  public static IServiceCollection AddUserModuleServices(
    this IServiceCollection services,
    List<Assembly> mediatRAssemblies,
    IConfiguration configuration,
    IHostEnvironment environment
    )
  {
    services.AddIdentityCore<ApplicationUser>()
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<UsersDbContext>()
      .AddDefaultTokenProviders();

    services.AddScoped<ITokenService, JwtTokenService>();

    services.Configure<EmailSettings>(configuration.GetSection("Email"));
    services.Configure<BulkUserCreationSettings>(configuration.GetSection("BulkUserCreation"));

    if (environment.IsDevelopment())
    {
      services.AddScoped<IEmailService, ConsoleEmailService>();
    }
    else
    {
      services.AddScoped<IEmailService, SmtpEmailService>();
    }

    services.AddScoped<ICsvParserService, CsvParserService>();
    services.AddScoped<IBulkCreateUsersService, BulkCreateUsersService>();

    mediatRAssemblies.Add(typeof(UsersModuleServiceExtensions).Assembly);
    return services;
  }
}
