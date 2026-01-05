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
      if (string.IsNullOrEmpty(token))
      {
        return;
      }

      var handler = new JwtSecurityTokenHandler();
      var jwtToken = handler.ReadJwtToken(token);
      var timeUntilExpiry = jwtToken.ValidTo - DateTime.UtcNow;

      // Only refresh if token expires soon AND user has been active recently
      if (timeUntilExpiry.TotalMinutes < RefreshBeforeExpiryMinutes)
      {
        var timeSinceLastActivity = DateTime.UtcNow - _lastActivity;

        // Only refresh if user was active in the last 5 minutes
        if (timeSinceLastActivity.TotalMinutes < 5)
        {
          var refreshToken = await authStateProvider.GetRefreshTokenAsync();
          if (!string.IsNullOrEmpty(refreshToken))
          {
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
        else
        {
          logger.LogInformation("Token not refreshed - user inactive for {Minutes} minutes", timeSinceLastActivity.TotalMinutes);
        }
      }
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "Error during token refresh");
    }
  }
}
