using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints;

internal class AssignApproverRoleEndpoint(UserManager<ApplicationUser> userManager) : EndpointWithoutRequest<EmptyResponse>
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
      return;
    }

    var requestingUsersTenantId = User.Claims.First(c => c.Type == "TenantId").Value;
    if (user.TenantId.ToString() != requestingUsersTenantId && !User.IsInRole("SystemAdmin"))
    {
      await Send.ForbiddenAsync(ct);
      return;
    }

    var approverRole = await userManager.GetRolesAsync(user);
    if (!approverRole.Contains("Approver"))
    {
      await userManager.AddToRoleAsync(user, "Approver");
    }

    await Send.OkAsync(ct);
  }
}
