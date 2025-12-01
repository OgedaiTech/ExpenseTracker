using ExpenseTracker.Core;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.Integrations;

public class CreateUserCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<CreateUserCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
  {

    var newUser = new ApplicationUser { UserName = request.Email, Email = request.Email, TenantId = request.TenantId };
    // TODO: Remove hardcoded password
    var result = await userManager.CreateAsync(newUser, "Password1!");
    return result.Succeeded
      ? new ServiceResult()
      : new ServiceResult("CANT_CREATE_USER");
  }
}
