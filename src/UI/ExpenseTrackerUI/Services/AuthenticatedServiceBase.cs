using System.Net.Http.Headers;

namespace ExpenseTrackerUI.Services;

public abstract class AuthenticatedServiceBase(IHttpClientFactory httpClientFactory, CustomAuthStateProvider authStateProvider)
{
  protected async Task<HttpClient> GetAuthenticatedClientAsync()
  {
    var client = httpClientFactory.CreateClient("AuthenticatedClient");

    var token = await authStateProvider.GetTokenAsync();
    if (!string.IsNullOrEmpty(token))
    {
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    return client;
  }
}
