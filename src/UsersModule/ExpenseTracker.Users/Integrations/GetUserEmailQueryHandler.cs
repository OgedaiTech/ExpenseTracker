using ExpenseTracker.Core;
using ExpenseTracker.Users.Contracts;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.Integrations;

public class GetUserEmailQueryHandler(UserManager<ApplicationUser> userManager)
  : IRequestHandler<GetUserEmailQuery, ServiceResult<UserEmailDto>>
{
  public async Task<ServiceResult<UserEmailDto>> Handle(GetUserEmailQuery request, CancellationToken cancellationToken)
  {
    var user = await userManager.FindByIdAsync(request.UserId.ToString());
    if (user is null)
    {
      return new ServiceResult<UserEmailDto>("User not found");
    }

    return new ServiceResult<UserEmailDto>(new UserEmailDto(
      user.Email ?? string.Empty,
      user.FirstName ?? string.Empty,
      user.LastName ?? string.Empty
    ));
  }
}
