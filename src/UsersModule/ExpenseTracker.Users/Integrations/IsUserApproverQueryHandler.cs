using ExpenseTracker.Core;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.Integrations;

public class IsUserApproverQueryHandler(UserManager<ApplicationUser> userManager)
  : IRequestHandler<IsUserApproverQuery, ServiceResult<bool>>
{
  public async Task<ServiceResult<bool>> Handle(IsUserApproverQuery request, CancellationToken cancellationToken)
  {
    var user = await userManager.FindByIdAsync(request.UserId.ToString());
    if (user is null)
    {
      return new ServiceResult<bool>("User not found", false);
    }

    if (user.TenantId != request.TenantId)
    {
      return new ServiceResult<bool>("User is not in the same tenant", false);
    }

    if (user.IsDeactivated)
    {
      return new ServiceResult<bool>("User is deactivated", false);
    }

    var roles = await userManager.GetRolesAsync(user);
    var isApprover = roles.Contains("Approver");

    return new ServiceResult<bool>(isApprover);
  }
}
