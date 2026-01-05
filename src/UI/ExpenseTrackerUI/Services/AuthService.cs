namespace ExpenseTrackerUI.Services;

public class AuthService(IHttpClientFactory httpClient)
{
  public async Task<string?> GetTokenAsync(string email, string password)
  {
    var client = httpClient.CreateClient("AuthenticatedClient");
    var response = await client.PostAsJsonAsync("/users/login", new { Email = email, Password = password });
    if (response.IsSuccessStatusCode)
    {
      var result = await response.Content.ReadFromJsonAsync<TokenResponse>();
      return result?.AccessToken;
    }
    return null;
  }
}
