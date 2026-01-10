using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.ApproverRole;

internal partial class AssignApproverRole(
  UserManager<ApplicationUser> userManager,
  ILogger<AssignApproverRole> logger) : EndpointWithoutRequest<EmptyResponse>
{
  public override void Configure()
  {
    Post("/users/{userId:guid}/roles/approver-role");
    Roles("TenantAdmin", "SystemAdmin");
  }

  public override async Task HandleAsync(CancellationToken ct)
  {
    var userId = Route<Guid>("userId");
    var user = await userManager.FindByIdAsync(userId.ToString());
    if (user is null)
    {
      await Send.NotFoundAsync(ct);
      LogUserNotFoundWhenTryingToAssignApproverRole(logger, userId, null);
      return;
    }

    var requestingUsersTenantId = User.Claims.First(c => c.Type == "TenantId").Value;
    if (user.TenantId.ToString() != requestingUsersTenantId && !User.IsInRole("SystemAdmin"))
    {
      await Send.ForbiddenAsync(ct);
      LogCannotAssignApproverRoleWhenTheyAreNotInSameTenant(
        logger,
        Guid.Parse(User.Claims.First(c => c.Type == "UserId").Value),
        userId,
        null);
      return;
    }

    var approverRole = await userManager.GetRolesAsync(user);
    if (!approverRole.Contains("Approver"))
    {
      await userManager.AddToRoleAsync(user, "Approver");
      LogAssignedApproverRole(logger, userId, Guid.Parse(User.Claims.First(c => c.Type == "UserId").Value), null);
    }

    await Send.OkAsync(ct);
  }
}
