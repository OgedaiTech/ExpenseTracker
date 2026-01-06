using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ExpenseTrackerUI.Services;

public class CustomAuthStateProvider(ProtectedLocalStorage localStorage) : AuthenticationStateProvider
{
  private const string TokenKey = "jwt_token";
  private const string RefreshTokenKey = "refresh_token";
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
        await localStorage.DeleteAsync(RefreshTokenKey);
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

  public async Task MarkUserAsAuthenticatedAsync(string accessToken, string refreshToken)
  {
    await localStorage.SetAsync(TokenKey, accessToken);
    await localStorage.SetAsync(RefreshTokenKey, refreshToken);

    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(accessToken);
    var claims = jwtToken.Claims.ToList();
    var identity = new ClaimsIdentity(claims, "jwt");
    var user = new ClaimsPrincipal(identity);

    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
  }

  public async Task MarkUserAsLoggedOutAsync()
  {
    await localStorage.DeleteAsync(TokenKey);
    await localStorage.DeleteAsync(RefreshTokenKey);
    NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(_anonymous)));
  }

  public async Task<string?> GetTokenAsync()
  {
    try
    {
      var tokenResult = await localStorage.GetAsync<string>(TokenKey);
      return tokenResult.Success ? tokenResult.Value : null;
    }
    catch
    {
      // Return null if we can't access storage (e.g., during prerendering)
      return null;
    }
  }

  public async Task<string?> GetRefreshTokenAsync()
  {
    try
    {
      var tokenResult = await localStorage.GetAsync<string>(RefreshTokenKey);
      return tokenResult.Success ? tokenResult.Value : null;
    }
    catch
    {
      // Return null if we can't access storage (e.g., during prerendering)
      return null;
    }
  }
}
