using ExpenseTracker.Core;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints.ListApprovers;

public class ListApproversService(UserManager<ApplicationUser> userManager) : IListApproversService
{
  public async Task<ServiceResult<List<ListApproversResponse>>> ListApproversAsync(string tenantId, CancellationToken cancellationToken)
  {
    var tenantGuid = Guid.Parse(tenantId);
    var usersInTenant = userManager.Users.Where(u => u.TenantId == tenantGuid && !u.IsDeactivated).ToList();

    var approvers = new List<ListApproversResponse>();
    foreach (var user in usersInTenant)
    {
      var roles = await userManager.GetRolesAsync(user);
      if (roles.Contains("Approver"))
      {
        approvers.Add(new ListApproversResponse
        {
          Id = Guid.Parse(user.Id),
          FirstName = user.FirstName ?? string.Empty,
          LastName = user.LastName ?? string.Empty,
          Email = user.Email ?? string.Empty
        });
      }
    }

    return new ServiceResult<List<ListApproversResponse>>(approvers);
  }
}
