using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Tenants.Endpoints.Create;

internal partial class CreateTenantEndpoint
{
  public static readonly Action<ILogger, string, string, Exception?> LogErrorWhenTryingToCreateTenant =
    LoggerMessage.Define<string, string>(
      LogLevel.Error,
      new EventId(0, "name: ErrorWhenTryingToCreateTenant"),
      "An error occurred when trying to create tenant with name {TenantName} with Reason: {Reason}");

  public static readonly Action<ILogger, string, Exception?> LogSuccessfullyCreatedTenant =
    LoggerMessage.Define<string>(
      LogLevel.Information,
      new EventId(0, "name: SuccessfullyCreatedTenant"),
      "Successfully created tenant with name {TenantName}");
}
