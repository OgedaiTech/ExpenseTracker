using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.ActivateUser;

internal partial class ActivateUserEndpoint(
    UserManager<ApplicationUser> userManager,
    ILogger<ActivateUserEndpoint> logger) : EndpointWithoutRequest<EmptyResponse>
{
  public override void Configure()
  {
    Put("/users/{userId:guid}/activate");
    Roles("SystemAdmin", "TenantAdmin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var userId = Route<Guid>("userId");
    var user = await userManager.FindByIdAsync(userId.ToString());

    if (user is null)
    {
      await Send.NotFoundAsync(ct);
      LogUserNotFound(logger, userId);
      return;
    }

    // Ensure tenant admins can only activate users in their tenant
    var requestingUsersTenantId = User.Claims.First(c => c.Type == "TenantId").Value;
    if (user.TenantId.ToString() != requestingUsersTenantId && !User.IsInRole("SystemAdmin"))
    {
      await Send.ForbiddenAsync(ct);
      LogCannotActivateUserFromDifferentTenant(logger, userId);
      return;
    }

    if (!user.IsDeactivated)
    {
      // Already activated, return OK
      await Send.OkAsync(ct);
      return;
    }

    user.IsDeactivated = false;
    var result = await userManager.UpdateAsync(user);

    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        AddError(error.Description);
      }
      ThrowIfAnyErrors();
    }

    LogUserActivated(logger, userId);
    await Send.OkAsync(ct);
  }

  [LoggerMessage(EventId = 200, Level = LogLevel.Warning, Message = "User not found when trying to activate: {UserId}")]
  private static partial void LogUserNotFound(ILogger logger, Guid userId);

  [LoggerMessage(EventId = 201, Level = LogLevel.Warning, Message = "Cannot activate user from different tenant: {UserId}")]
  private static partial void LogCannotActivateUserFromDifferentTenant(ILogger logger, Guid userId);

  [LoggerMessage(EventId = 202, Level = LogLevel.Information, Message = "User activated: {UserId}")]
  private static partial void LogUserActivated(ILogger logger, Guid userId);
}
