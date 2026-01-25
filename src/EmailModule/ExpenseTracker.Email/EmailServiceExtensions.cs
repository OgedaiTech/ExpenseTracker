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

    // In Development, use SMTP if configured, otherwise fall back to Console
    if (environment.IsDevelopment())
    {
      var emailSettings = configuration.GetSection("Email").Get<EmailSettings>();

      // If Gmail SMTP is configured (via user secrets), use SMTP; otherwise use Console
      if (!string.IsNullOrEmpty(emailSettings?.SmtpHost) &&
          emailSettings.SmtpHost != "smtp.example.com" &&
          !string.IsNullOrEmpty(emailSettings.SmtpUsername))
      {
        services.AddScoped<IEmailService, SmtpEmailService>();
      }
      else
      {
        services.AddScoped<IEmailService, ConsoleEmailService>();
      }
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
