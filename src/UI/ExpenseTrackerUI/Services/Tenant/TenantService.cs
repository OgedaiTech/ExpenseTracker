using System.Net;
using System.Text.Json.Serialization;
using ExpenseTrackerUI.Services.Authentication;

namespace ExpenseTrackerUI.Services.Tenant;

public class TenantService(IHttpClientFactory httpClientFactory, CustomAuthStateProvider authStateProvider)
  : AuthenticatedServiceBase(httpClientFactory, authStateProvider)
{
  public async Task<ServiceResult<CreateTenantResponse>> CreateTenantAsync(CreateTenantDto request)
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();

      var response = await client.PostAsJsonAsync("/tenants", request);

      if (response.IsSuccessStatusCode)
      {
        // Check if response has content
        var content = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(content))
        {
          // Backend returns empty response, create a success result without data
          return ServiceResult<CreateTenantResponse>.Success(new CreateTenantResponse
          {
            Id = Guid.Empty,
            Name = request.Name,
            Code = request.Code ?? string.Empty
          });
        }

        var result = await response.Content.ReadFromJsonAsync<CreateTenantResponse>();
        return ServiceResult<CreateTenantResponse>.Success(result!);
      }

      // Try to parse ProblemDetails for error information
      try
      {
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        return ServiceResult<CreateTenantResponse>.Failure(
          response.StatusCode,
          problemDetails?.Detail,
          null
        );
      }
      catch
      {
        // Fallback if response is not ProblemDetails
        var errorContent = await response.Content.ReadAsStringAsync();
        return ServiceResult<CreateTenantResponse>.Failure(
          response.StatusCode,
          null,
          errorContent
        );
      }
    }
    catch (Exception ex)
    {
      return ServiceResult<CreateTenantResponse>.Failure(
        HttpStatusCode.InternalServerError,
        null,
        ex.Message
      );
    }
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
