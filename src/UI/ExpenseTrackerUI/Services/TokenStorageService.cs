namespace ExpenseTrackerUI.Services;

public class TokenStorageService(IHttpContextAccessor httpContextAccessor) : ITokenStorageService
{
  private const string AccessTokenKey = "accessToken";
  private const string RefreshTokenKey = "refreshToken";

  public Task ClearTokensAsync()
  {
    var session = httpContextAccessor.HttpContext?.Session;
    if (session is not null)
    {
      session.Remove(AccessTokenKey);
      session.Remove(RefreshTokenKey);
    }
    return Task.CompletedTask;
  }

  public Task<string?> GetAccessTokenAsync()
  {
    var session = httpContextAccessor.HttpContext?.Session;
    var token = session?.GetString(AccessTokenKey);
    return Task.FromResult(token);
  }

  public Task<string?> GetRefreshTokenAsync()
  {
    var session = httpContextAccessor.HttpContext?.Session;
    var token = session?.GetString(RefreshTokenKey);
    return Task.FromResult(token);
  }

  public Task StoreTokensAsync(string accessToken, string refreshToken)
  {
    var session = httpContextAccessor.HttpContext?.Session;
    if (session is not null)
    {
      session.SetString(AccessTokenKey, accessToken);
      session.SetString(RefreshTokenKey, refreshToken);
    }
    return Task.CompletedTask;
  }
}
