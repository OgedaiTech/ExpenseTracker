using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ExpenseTracker.Email;

public static class EmailServiceExtensions
{
  public static IServiceCollection AddEmailServices(
      this IServiceCollection services,
      List<Assembly> mediatRAssemblies,
      IConfiguration configuration,
      IHostEnvironment environment)
  {
    services.Configure<EmailSettings>(configuration.GetSection("Email"));
    services.Configure<InvitationEmailSettings>(configuration.GetSection("InvitationEmail"));

    if (environment.IsDevelopment())
    {
      services.AddScoped<IEmailService, ConsoleEmailService>();
    }
    else
    {
      services.AddScoped<IEmailService, SmtpEmailService>();
    }

    // Add EmailModule assembly for MediatR handler discovery
    mediatRAssemblies.Add(typeof(EmailServiceExtensions).Assembly);

    return services;
  }
}
