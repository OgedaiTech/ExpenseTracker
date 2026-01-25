using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

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

      // Try FastEndpoints error format first
      try
      {
        var fastEndpointsError = JsonSerializer.Deserialize<FastEndpointsErrorResponse>(errorContent);
        if (fastEndpointsError?.Errors != null && fastEndpointsError.Errors.Count > 0)
        {
          // Collect all error messages
          var errorMessages = new List<string>();
          foreach (var errorGroup in fastEndpointsError.Errors)
          {
            errorMessages.AddRange(errorGroup.Value);
          }

          if (errorMessages.Count > 0)
          {
            // Join all error messages with line breaks
            var combinedMessage = string.Join(" ", errorMessages);
            return ServiceResult<bool>.Failure(
                response.StatusCode,
                null,
                combinedMessage
            );
          }
        }
      }
      catch
      {
        // Not FastEndpoints format, try ProblemDetails
      }

      // Try ProblemDetails format
      try
      {
        var problemDetails = JsonSerializer.Deserialize<ProblemDetailsResponse>(errorContent);
        return ServiceResult<bool>.Failure(
            response.StatusCode,
            problemDetails?.Detail,
            problemDetails?.Title
        );
      }
      catch
      {
        // Fallback to raw error content
        return ServiceResult<bool>.Failure(
            response.StatusCode,
            null,
            errorContent
        );
      }
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

  private sealed class FastEndpointsErrorResponse
  {
    [JsonPropertyName("statusCode")]
    public int? StatusCode { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("errors")]
    public Dictionary<string, string[]>? Errors { get; set; }
  }

  private sealed class ProblemDetailsResponse
  {
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("title")]
    public string? Title { get; set; }

    [JsonPropertyName("status")]
    public int? Status { get; set; }

    [JsonPropertyName("detail")]
    public string? Detail { get; set; }

    [JsonPropertyName("instance")]
    public string? Instance { get; set; }
  }
}
