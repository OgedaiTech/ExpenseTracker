using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.ApproverRole;

internal partial class RemoveApproverRole(
  UserManager<ApplicationUser> userManager,
  ILogger<RemoveApproverRole> logger) : EndpointWithoutRequest<EmptyResponse>
{
  public override void Configure()
  {
    Delete("/users/{userId:guid}/roles/approver-role");
    Roles("TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var userId = Route<Guid>("userId");
    var user = await userManager.FindByIdAsync(userId.ToString());
    if (user is null)
    {
      await Send.NotFoundAsync(ct);
      LogUserNotFoundWhenTryingToRemoveApproverRole(logger, userId, null);
      return;
    }

    var requestingUsersTenantId = User.Claims.First(c => c.Type == "TenantId").Value;
    if (user.TenantId.ToString() != requestingUsersTenantId && !User.IsInRole("SystemAdmin"))
    {
      await Send.ForbiddenAsync(ct);
      LogCannotRemoveApproverRoleWhenTheyAreNotInSameTenant(
        logger,
        Guid.Parse(User.Claims.First(c => c.Type == "UserId").Value),
        userId,
        null);
      return;
    }

    var approverRole = await userManager.GetRolesAsync(user);
    if (approverRole.Contains("Approver"))
    {
      await userManager.RemoveFromRoleAsync(user, "Approver");
      LogRemovedApproverRole(
        logger,
        userId,
        Guid.Parse(User.Claims.First(c => c.Type == "UserId").Value),
        null);
    }

    await Send.OkAsync(ct);
  }
}
