namespace ExpenseTrackerUI.Services.Authentication;

public class AuthService(IHttpClientFactory httpClient)
{
  public async Task<TokenResponse?> GetTokenAsync(string email, string password)
  {
    var client = httpClient.CreateClient(string.Empty);
    var response = await client.PostAsJsonAsync("/users/login", new { Email = email, Password = password });
    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }
    return null;
  }

  public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken)
  {
    var client = httpClient.CreateClient(string.Empty);
    var response = await client.PostAsJsonAsync("/users/refresh", new { RefreshToken = refreshToken });
    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<TokenResponse>();
    }
    return null;
  }
}
