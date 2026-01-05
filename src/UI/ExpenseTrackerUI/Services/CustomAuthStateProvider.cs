using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ExpenseTrackerUI.Services;

public class CustomAuthStateProvider(ProtectedLocalStorage localStorage) : AuthenticationStateProvider
{
  private const string TokenKey = "jwt_token";
  private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());

  public override async Task<AuthenticationState> GetAuthenticationStateAsync()
  {
    try
    {
      var tokenResult = await localStorage.GetAsync<string>(TokenKey);

      if (!tokenResult.Success || string.IsNullOrEmpty(tokenResult.Value))
      {
        return new AuthenticationState(_anonymous);
      }

      var token = tokenResult.Value;
      var handler = new JwtSecurityTokenHandler();
      var jwtToken = handler.ReadJwtToken(token);

      // Check if token is expired
      if (jwtToken.ValidTo < DateTime.UtcNow)
      {
        await localStorage.DeleteAsync(TokenKey);
        return new AuthenticationState(_anonymous);
      }

      var claims = jwtToken.Claims.ToList();
      var identity = new ClaimsIdentity(claims, "jwt");
      var user = new ClaimsPrincipal(identity);

      return new AuthenticationState(user);
    }
    catch
    {
      return new AuthenticationState(_anonymous);
    }
  }

  public async Task MarkUserAsAuthenticatedAsync(string token)
  {
    await localStorage.SetAsync(TokenKey, token);

    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    var claims = jwtToken.Claims.ToList();
    var identity = new ClaimsIdentity(claims, "jwt");
    var user = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }

  public async Task MarkUserAsLoggedOutAsync()
  {
    await localStorage.DeleteAsync(TokenKey);
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
  }
}
