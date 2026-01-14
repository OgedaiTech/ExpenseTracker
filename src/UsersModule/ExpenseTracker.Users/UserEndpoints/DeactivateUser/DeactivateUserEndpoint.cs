using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.DeactivateUser;

internal partial class DeactivateUserEndpoint(
    UserManager<ApplicationUser> userManager,
    ILogger<DeactivateUserEndpoint> logger) : EndpointWithoutRequest<EmptyResponse>
{
  public override void Configure()
  {
    Put("/users/{userId:guid}/deactivate");
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

    // Ensure tenant admins can only deactivate users in their tenant
    var requestingUsersTenantId = User.Claims.First(c => c.Type == "TenantId").Value;
    if (user.TenantId.ToString() != requestingUsersTenantId && !User.IsInRole("SystemAdmin"))
    {
      await Send.ForbiddenAsync(ct);
      LogCannotDeactivateUserFromDifferentTenant(logger, userId);
      return;
    }

    if (user.IsDeactivated)
    {
      // Already deactivated, return OK
      await Send.OkAsync(ct);
      return;
    }

    user.IsDeactivated = true;
    var result = await userManager.UpdateAsync(user);

    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        AddError(error.Description);
      }
      ThrowIfAnyErrors();
    }

    LogUserDeactivated(logger, userId);
    await Send.OkAsync(ct);
  }

  [LoggerMessage(EventId = 210, Level = LogLevel.Warning, Message = "User not found when trying to deactivate: {UserId}")]
  private static partial void LogUserNotFound(ILogger logger, Guid userId);

  [LoggerMessage(EventId = 211, Level = LogLevel.Warning, Message = "Cannot deactivate user from different tenant: {UserId}")]
  private static partial void LogCannotDeactivateUserFromDifferentTenant(ILogger logger, Guid userId);

  [LoggerMessage(EventId = 212, Level = LogLevel.Information, Message = "User deactivated: {UserId}")]
  private static partial void LogUserDeactivated(ILogger logger, Guid userId);
}
