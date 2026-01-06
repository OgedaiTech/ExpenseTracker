using System.IdentityModel.Tokens.Jwt;

namespace ExpenseTrackerUI.Services;

public class TokenRefreshService(
    CustomAuthStateProvider authStateProvider,
    AuthService authService,
    ILogger<TokenRefreshService> logger)
{
  private DateTime _lastActivity = DateTime.UtcNow;
  private const int RefreshBeforeExpiryMinutes = 2; // Refresh 2 minutes before expiry

  public void RecordActivity()
  {
    _lastActivity = DateTime.UtcNow;
  }

  public async Task RefreshTokenIfNeededAsync()
  {
    try
    {
      var token = await authStateProvider.GetTokenAsync();
      if (string.IsNullOrEmpty(token)) return;

      var timeUntilExpiry = GetTimeUntilExpiry(token);
      if (timeUntilExpiry.TotalMinutes >= RefreshBeforeExpiryMinutes) return;

      await HandleTokenRefreshAsync();
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error during token refresh");
    }
  }

  private static TimeSpan GetTimeUntilExpiry(string token)
  {
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(token);
    return jwtToken.ValidTo - DateTime.UtcNow;
  }

  private async Task HandleTokenRefreshAsync()
  {
    var timeSinceLastActivity = DateTime.UtcNow - _lastActivity;

    if (timeSinceLastActivity.TotalMinutes < 5)
    {
      await PerformTokenRefreshAsync();
    }
    else if (logger.IsEnabled(LogLevel.Information))
    {
      logger.LogInformation("Token not refreshed - user inactive for {Minutes} minutes", timeSinceLastActivity.TotalMinutes);
    }
  }

  private async Task PerformTokenRefreshAsync()
  {
    var refreshToken = await authStateProvider.GetRefreshTokenAsync();
    if (string.IsNullOrEmpty(refreshToken)) return;

    var tokenResponse = await authService.RefreshTokenAsync(refreshToken);
    if (tokenResponse is not null)
    {
      await authStateProvider.MarkUserAsAuthenticatedAsync(tokenResponse.AccessToken, tokenResponse.RefreshToken);
      logger.LogInformation("Token refreshed successfully due to user activity");
    }
    else
    {
      logger.LogWarning("Failed to refresh token");
    }
  }
}
