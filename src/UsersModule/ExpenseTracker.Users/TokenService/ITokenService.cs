namespace ExpenseTracker.Users.TokenService;

public interface ITokenService
{
  Task<TokenResponse> GenerateTokensAsync(ApplicationUser user);
  Task<TokenResponse?> RefreshTokenAsync(string refreshToken);
  Task RevokeTokenAsync(string refreshToken);
}
