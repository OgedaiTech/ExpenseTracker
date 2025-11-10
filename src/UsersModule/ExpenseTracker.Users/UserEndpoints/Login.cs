using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints;

internal class Login(UserManager<ApplicationUser> userManager) : Endpoint<UserLoginRequest>
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

    var loginSuccesful = await userManager.CheckPasswordAsync(user, req.Password);
    if (!loginSuccesful)
    {
      await Send.UnauthorizedAsync(ct);
      return;
    }

    var jwtSecret = Config["Auth:JwtSecret"]!;
    var token = JwtBearer.CreateToken(options =>
    {
      options.SigningKey = jwtSecret;
      options.User["EmailAddress"] = user.Email!;
      options.User["UserId"] = user.Id;
    });

    await Send.OkAsync(new { Token = token }, ct);
  }
}
