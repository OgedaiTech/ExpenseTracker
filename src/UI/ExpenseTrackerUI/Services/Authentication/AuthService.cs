using System.Net;
using System.Text.Json;

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

  public async Task<ServiceResult<bool>> SetPasswordAsync(string email, string token, string newPassword, string confirmPassword)
  {
    try
    {
      var client = httpClient.CreateClient(string.Empty);
      var response = await client.PostAsJsonAsync("/users/set-password", new
      {
        Email = email,
        Token = token,
        NewPassword = newPassword,
        ConfirmPassword = confirmPassword
      });

      if (response.IsSuccessStatusCode)
      {
        return ServiceResult<bool>.Success(true);
      }

      // Try to parse error response
      var errorContent = await response.Content.ReadAsStringAsync();

      var problemDetails = JsonSerializer.Deserialize<ProblemDetailsResponse>(errorContent);
      return ServiceResult<bool>.Failure(
          response.StatusCode,
          null,
          problemDetails?.Detail
      );
    }
    catch (Exception ex)
    {
      return ServiceResult<bool>.Failure(
          HttpStatusCode.InternalServerError,
          null,
          ex.Message
      );
    }
  }

  public async Task<ServiceResult<bool>> ForgotPasswordAsync(string email)
  {
    try
    {
      var client = httpClient.CreateClient(string.Empty);
      var response = await client.PostAsJsonAsync("/users/forgot-password", new { Email = email });

      if (response.IsSuccessStatusCode)
      {
        return ServiceResult<bool>.Success(true);
      }

      // Try to parse error response
      var errorContent = await response.Content.ReadAsStringAsync();

      var problemDetails = JsonSerializer.Deserialize<ProblemDetailsResponse>(errorContent);
      return ServiceResult<bool>.Failure(
          response.StatusCode,
          null,
          problemDetails?.Detail
      );
    }
    catch (Exception ex)
    {
      return ServiceResult<bool>.Failure(
          HttpStatusCode.InternalServerError,
          null,
          ex.Message
      );
    }
  }
}
