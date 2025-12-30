using ExpenseTracker.Users.TokenService;
using FastEndpoints;

namespace ExpenseTracker.Users.UserEndpoints;

public class RefreshTokenRequest
{
  public string RefreshToken { get; set; } = default!;
}

internal class RefreshToken(ITokenService tokenService)
    : Endpoint<RefreshTokenRequest>
{
  public override void Configure()
  {
    Post("/users/refresh");
    AllowAnonymous();
  }

  public override async Task HandleAsync(RefreshTokenRequest req, CancellationToken ct)
  {
    var tokenResponse = await tokenService.RefreshTokenAsync(req.RefreshToken);

    if (tokenResponse is null)
    {
      await Send.UnauthorizedAsync(ct);
      return;
    }

    await Send.OkAsync(tokenResponse, ct);
  }
}
