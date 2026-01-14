using ExpenseTracker.Users.TokenService;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints.LoginLogout;

internal class Login(
  UserManager<ApplicationUser> userManager,
  ITokenService tokenService) : Endpoint<UserLoginRequest>
{
  public override void Configure()
  {
    Post("/users/login");
    AllowAnonymous();
  }

  public override async Task HandleAsync(UserLoginRequest req, CancellationToken ct)
  {
    var user = await userManager.FindByEmailAsync(req.Email);
    if (user is null)
    {
      await Send.UnauthorizedAsync(ct);
      return;
    }

    if (user.IsDeactivated)
    {
      await Send.UnauthorizedAsync(ct);
      return;
    }

    var loginSuccesful = await userManager.CheckPasswordAsync(user, req.Password);
    if (!loginSuccesful)
    {
      await Send.UnauthorizedAsync(ct);
      return;
    }

    var tokenResponse = await tokenService.GenerateTokensAsync(user);

    await Send.OkAsync(tokenResponse, ct);
  }
}
