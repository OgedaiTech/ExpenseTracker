using System;

namespace ExpenseTracker.Users;

public interface ITokenService
{
  Task<TokenResponse> GenerateTokensAsync(ApplicationUser user);
  Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
  Task RevokeTokenAsync(string refreshToken);
}

public class TokenResponse
{
  public string AccessToken { get; set; } = default!;
  public string RefreshToken { get; set; } = default!;
  public string TokenType { get; set; } = "Bearer";
  public int ExpiresIn { get; set; } // Seconds until expiration
}
