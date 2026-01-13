using ExpenseTracker.Core;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.Integrations;

public class CreateTenantAdminUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CreateUserCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {

    var newUser = new ApplicationUser
    {
      UserName = request.Email,
      Email = request.Email,
      TenantId = request.TenantId,
      FirstName = request.FirstName,
      LastName = request.LastName,
      NationalIdentityNo = request.NationalIdentityNo,
      TaxIdNo = request.TaxIdNo,
      EmployeeId = request.EmployeeId,
      Title = request.Title
    };

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
    else if (result.Errors.Contains(result.Errors.FirstOrDefault(e => e.Code == "DuplicateUserName")))
    {
      return new ServiceResult("TENANT_ADMIN_USER_EMAIL_ALREADY_EXISTS");
    }
    else
    {
      return new ServiceResult("CANT_CREATE_USER");
    }
  }
}
