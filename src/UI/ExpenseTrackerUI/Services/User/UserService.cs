using System.Net;
using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using ExpenseTrackerUI.Services.Authentication;
using Microsoft.AspNetCore.Components.Forms;

namespace ExpenseTrackerUI.Services.User;

public class UserService(IHttpClientFactory httpClientFactory, CustomAuthStateProvider authStateProvider)
    : AuthenticatedServiceBase(httpClientFactory, authStateProvider)
{
  private const long MaxFileSize = 1024 * 1024; // 1MB

  public async Task<ServiceResult<BulkCreateUsersResponse>> BulkCreateUsersAsync(IBrowserFile file)
  {
    try
    {
      // Client-side file size validation
      if (file.Size > MaxFileSize)
      {
        return ServiceResult<BulkCreateUsersResponse>.Failure(
            HttpStatusCode.BadRequest,
            "FILE_TOO_LARGE",
            $"File size exceeds the maximum allowed size of {MaxFileSize / 1024 / 1024}MB"
        );
      }

      var client = await GetAuthenticatedClientAsync();

      // Create multipart form data content
      using var content = new MultipartFormDataContent();
      var streamContent = new StreamContent(file.OpenReadStream(MaxFileSize));
      streamContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
      content.Add(streamContent, "CsvFile", file.Name);

      var response = await client.PostAsync("/users/bulk", content);

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<BulkCreateUsersResponse>();
        return ServiceResult<BulkCreateUsersResponse>.Success(result!);
      }

      // Try to parse ProblemDetails for error information
      try
      {
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        return ServiceResult<BulkCreateUsersResponse>.Failure(
            response.StatusCode,
            problemDetails?.Detail,
            null
        );
      }
      catch
      {
        // Fallback if response is not ProblemDetails
        var errorContent = await response.Content.ReadAsStringAsync();
        return ServiceResult<BulkCreateUsersResponse>.Failure(
            response.StatusCode,
            null,
            errorContent
        );
      }
    }
    catch (Exception ex)
    {
      return ServiceResult<BulkCreateUsersResponse>.Failure(
          HttpStatusCode.InternalServerError,
          null,
          ex.Message
      );
    }
  }

  public async Task<ServiceResult<CreateUserWithInvitationResponse>> CreateUserWithInvitationAsync(CreateUserWithInvitationDto request)
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();
      var response = await client.PostAsJsonAsync("/users/invite", request);

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<CreateUserWithInvitationResponse>();
        return ServiceResult<CreateUserWithInvitationResponse>.Success(result!);
      }

      // Try to parse error response
      var errorContent = await response.Content.ReadAsStringAsync();

      // Try FastEndpoints error format first
      try
      {
        var fastEndpointsError = System.Text.Json.JsonSerializer.Deserialize<FastEndpointsErrorResponse>(errorContent);
        if (fastEndpointsError?.Errors != null && fastEndpointsError.Errors.Count > 0)
        {
          // FastEndpoints returns errors under "GeneralErrors" key
          // The actual error code is in the array values
          if (fastEndpointsError.Errors.TryGetValue("GeneralErrors", out var generalErrors) && generalErrors.Length > 0)
          {
            var errorCode = generalErrors[0]; // The error code is the first item in the array
            return ServiceResult<CreateUserWithInvitationResponse>.Failure(
                response.StatusCode,
                errorCode,
                null
            );
          }

          // Fallback: if not under GeneralErrors, use the first error
          var firstError = fastEndpointsError.Errors.First();
          var fallbackErrorCode = firstError.Value.FirstOrDefault();
          return ServiceResult<CreateUserWithInvitationResponse>.Failure(
              response.StatusCode,
              fallbackErrorCode,
              null
          );
        }
      }
      catch
      {
        // Not FastEndpoints format, try ProblemDetails
      }

      // Try ProblemDetails format
      try
      {
        var problemDetails = System.Text.Json.JsonSerializer.Deserialize<ProblemDetailsResponse>(errorContent);
        return ServiceResult<CreateUserWithInvitationResponse>.Failure(
            response.StatusCode,
            problemDetails?.Detail,
            problemDetails?.Title
        );
      }
      catch
      {
        // Fallback to raw error content
        return ServiceResult<CreateUserWithInvitationResponse>.Failure(
            response.StatusCode,
            null,
            errorContent
        );
      }
    }
    catch (Exception ex)
    {
      return ServiceResult<CreateUserWithInvitationResponse>.Failure(
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

  public async Task<ServiceResult<ListUsersResponse>> GetUsersAsync(
      string? cursor = null,
      int pageSize = 2,
      string? searchQuery = null,
      bool? isDeactivated = null)
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();

      // Build query parameters
      var queryParams = new List<string>();

      if (!string.IsNullOrEmpty(cursor))
      {
        queryParams.Add($"cursor={Uri.EscapeDataString(cursor)}");
      }

      queryParams.Add($"pageSize={pageSize}");

      if (!string.IsNullOrEmpty(searchQuery))
      {
        queryParams.Add($"searchQuery={Uri.EscapeDataString(searchQuery)}");
      }

      if (isDeactivated.HasValue)
      {
        queryParams.Add($"isDeactivated={isDeactivated.Value.ToString().ToLower()}");
      }

      var queryString = queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : "";
      var response = await client.GetAsync($"/users{queryString}");

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<ListUsersResponse>();
        return ServiceResult<ListUsersResponse>.Success(result!);
      }

      // Try to parse error response
      try
      {
        var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
        return ServiceResult<ListUsersResponse>.Failure(
            response.StatusCode,
            problemDetails?.Detail,
            problemDetails?.Title
        );
      }
      catch
      {
        var errorContent = await response.Content.ReadAsStringAsync();
        return ServiceResult<ListUsersResponse>.Failure(
            response.StatusCode,
            null,
            errorContent
        );
      }
    }
    catch (Exception ex)
    {
      return ServiceResult<ListUsersResponse>.Failure(
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
