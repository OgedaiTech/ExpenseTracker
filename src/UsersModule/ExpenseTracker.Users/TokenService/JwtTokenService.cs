using System.Security.Cryptography;
using FastEndpoints.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace ExpenseTracker.Users.TokenService;

public class JwtTokenService(UserManager<ApplicationUser> userManager,
    IConfiguration configuration) : ITokenService
{
  private const string RefreshTokenProvider = "RefreshTokenProvider";
  private const string RefreshTokenPurpose = "RefreshToken";
  private const int AccessTokenExpirationMinutes = 15;
  private const int RefreshTokenExpirationDays = 7;

  public async Task<TokenResponse> GenerateTokensAsync(ApplicationUser user)
  {
    var jwtSecret = configuration["Auth:JwtSecret"]!;
    var jwtIssuer = configuration["Auth:JwtIssuer"]!;
    var jwtAudience = configuration["Auth:JwtAudience"]!;
    var roles = await userManager.GetRolesAsync(user);

    var accessToken = JwtBearer.CreateToken(options =>
    {
      options.SigningKey = jwtSecret;
      options.ExpireAt = DateTime.UtcNow.AddMinutes(AccessTokenExpirationMinutes);
      options.Issuer = jwtIssuer;
      options.Audience = jwtAudience;
      options.User["EmailAddress"] = user.Email!;
      options.User["UserId"] = user.Id;
      options.User["TenantId"] = user.TenantId.ToString();
      options.User.Roles.AddRange(roles);
    });

    var refreshToken = GenerateRefreshToken(user.Id);

    await userManager.SetAuthenticationTokenAsync(
        user,
        RefreshTokenProvider,
        RefreshTokenPurpose,
        refreshToken
    );

    return new TokenResponse
    {
      AccessToken = accessToken,
      RefreshToken = refreshToken,
      ExpiresIn = AccessTokenExpirationMinutes * 60 // Convert to seconds
    };
  }

  public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
  {
    var parts = refreshToken.Split(':');
    if (parts.Length != 2)
    {
      return null;
    }

    var userId = parts[0];
    var user = await userManager.FindByIdAsync(userId);

    if (user is null)
    {
      return null;
    }

    var storedToken = await userManager.GetAuthenticationTokenAsync(
        user,
        RefreshTokenProvider,
        RefreshTokenPurpose
    );

    if (storedToken != refreshToken)
    {
      return null;
    }

    // Generate new tokens
    return await GenerateTokensAsync(user);
  }

  public async Task RevokeTokenAsync(string refreshToken)
  {
    var parts = refreshToken.Split(':');
    if (parts.Length != 2)
    {
      return;
    }

    var userId = parts[0];
    var user = await userManager.FindByIdAsync(userId);

    if (user is not null)
    {
      await userManager.RemoveAuthenticationTokenAsync(
          user,
          RefreshTokenProvider,
          RefreshTokenPurpose
      );
    }
  }

  private static string GenerateRefreshToken(string userId)
  {
    var randomBytes = new byte[64];
    using var rng = RandomNumberGenerator.Create();
    rng.GetBytes(randomBytes);
    var token = Convert.ToBase64String(randomBytes);

    var expirationDate = DateTime.UtcNow.AddDays(RefreshTokenExpirationDays);

    return $"{userId}:{expirationDate.Ticks}:{token}";
  }
}
