using ExpenseTracker.Core;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.Integrations;

public class CreateTenantAdminUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CreateUserCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {

    var newUser = new ApplicationUser { UserName = request.Email, Email = request.Email, TenantId = request.TenantId };

    var result = await userManager.CreateAsync(newUser, request.Password);

    if (result.Succeeded)
    {
      var roleResult = await userManager.AddToRoleAsync(newUser, "TenantAdmin");
      if (!roleResult.Succeeded)
      {
        await userManager.DeleteAsync(newUser);
        return new ServiceResult("CANT_ASSIGN_ROLE");
      }
      return new ServiceResult();
    }
    else
    {
      return new ServiceResult("CANT_CREATE_USER");
    }
  }
}
