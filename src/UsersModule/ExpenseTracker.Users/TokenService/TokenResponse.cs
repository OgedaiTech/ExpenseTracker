namespace ExpenseTracker.Users.TokenService;

public class TokenResponse
{
  public string AccessToken { get; set; } = default!;
  public string RefreshToken { get; set; } = default!;
  public string TokenType { get; set; } = "Bearer";
  public int ExpiresIn { get; set; } // Seconds until expiration
}
