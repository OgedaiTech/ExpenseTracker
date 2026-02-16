using System.Text.Json.Serialization;

namespace ExpenseTracker.Accounting.Providers.Parasut;

public class ParasutTokenResponse
{
  [JsonPropertyName("access_token")]
  public string AccessToken { get; set; } = string.Empty;

  [JsonPropertyName("token_type")]
  public string TokenType { get; set; } = string.Empty;

  [JsonPropertyName("expires_in")]
  public int ExpiresIn { get; set; }

  [JsonPropertyName("refresh_token")]
  public string RefreshToken { get; set; } = string.Empty;
}
