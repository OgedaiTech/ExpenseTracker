using System.Net;
using System.Net.Http.Headers;

namespace ExpenseTrackerUI.Services;

public class AuthenticatedHttpClientHandler(
    ITokenStorageService tokenStorage,
    IHttpClientFactory httpClientFactory,
    IConfiguration configuration) : DelegatingHandler
{
  protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
  {
    var accessToken = await tokenStorage.GetAccessTokenAsync();
    if (!string.IsNullOrEmpty(accessToken))
    {
      request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    }

    var response = await base.SendAsync(request, cancellationToken);

    // If 401, try to refresh token
    if (response.StatusCode == HttpStatusCode.Unauthorized)
    {
      var refreshToken = await tokenStorage.GetRefreshTokenAsync();

      if (!string.IsNullOrEmpty(refreshToken))
      {
        var refreshed = await RefreshTokenAsync(refreshToken, cancellationToken);
        if (refreshed)
        {
          // Retry the original request with new token
          var newAccessToken = await tokenStorage.GetAccessTokenAsync();
          request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccessToken);
          response = await base.SendAsync(request, cancellationToken);
        }
      }
    }

    return response;
  }

  private async Task<bool> RefreshTokenAsync(string refreshToken, CancellationToken cancellationToken)
  {
    try
    {
      var client = httpClientFactory.CreateClient();
      var refreshRequest = new { RefreshToken = refreshToken };
      var apiBaseUrl = configuration["ApiBaseUrl"];

      var response = await client.PostAsJsonAsync(
          $"{apiBaseUrl}/users/refresh",
          refreshRequest,
          cancellationToken);

      if (response.IsSuccessStatusCode)
      {
        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>(cancellationToken);

        if (tokenResponse is not null)
        {
          await tokenStorage.StoreTokensAsync(
              tokenResponse.AccessToken,
              tokenResponse.RefreshToken);
          return true;
        }
      }
    }
    catch
    {
      // Refresh failed, user needs to login again
    }

    return false;
  }
}
