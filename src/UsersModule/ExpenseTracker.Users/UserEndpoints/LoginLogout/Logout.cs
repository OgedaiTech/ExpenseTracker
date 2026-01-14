using ExpenseTracker.Users.TokenService;
using FastEndpoints;

namespace ExpenseTracker.Users.UserEndpoints.LoginLogout;

internal class Logout(ITokenService tokenService)
    : Endpoint<RefreshTokenRequest>
{
  public override void Configure()
  {
    Post("/users/logout");
    AllowAnonymous();
  }

  public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
  {
    await tokenService.RevokeTokenAsync(req.RefreshToken);
    await Send.NoContentAsync(ct);
  }
}
