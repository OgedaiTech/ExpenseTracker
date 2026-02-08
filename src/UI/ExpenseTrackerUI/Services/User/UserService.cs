using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
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

      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return ServiceResult<BulkCreateUsersResponse>.Failure(
          response.StatusCode,
          null,
          problemDetails?.Detail
      );
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

      var problemDetails = JsonSerializer.Deserialize<ProblemDetailsResponse>(errorContent);
      return ServiceResult<CreateUserWithInvitationResponse>.Failure(
          response.StatusCode,
          null,
          message: problemDetails?.Detail
      );
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

      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return ServiceResult<ListUsersResponse>.Failure(
          response.StatusCode,
          null,
          problemDetails?.Detail
      );
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

  public async Task<ServiceResult<UpdateUserResponse>> UpdateUserAsync(Guid userId, UpdateUserDto request)
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();
      var response = await client.PutAsJsonAsync($"/users/{userId}", request);

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<UpdateUserResponse>();
        return ServiceResult<UpdateUserResponse>.Success(result!);
      }

      // Try to parse error response
      var errorContent = await response.Content.ReadAsStringAsync();

      var problemDetails = JsonSerializer.Deserialize<ProblemDetailsResponse>(errorContent);
      return ServiceResult<UpdateUserResponse>.Failure(
          response.StatusCode,
          null,
          problemDetails?.Detail
      );
    }
    catch (Exception ex)
    {
      return ServiceResult<UpdateUserResponse>.Failure(
          HttpStatusCode.InternalServerError,
          null,
          ex.Message
      );
    }
  }

  public async Task<ServiceResult<bool>> AssignApproverRoleAsync(Guid userId)
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();
      var response = await client.PostAsync($"/users/{userId}/roles/approver-role", null);

      if (response.IsSuccessStatusCode)
      {
        return ServiceResult<bool>.Success(true);
      }

      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
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

  public async Task<ServiceResult<bool>> RemoveApproverRoleAsync(Guid userId)
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();
      var response = await client.DeleteAsync($"/users/{userId}/roles/approver-role");

      if (response.IsSuccessStatusCode)
      {
        return ServiceResult<bool>.Success(true);
      }

      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
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

  public async Task<ServiceResult<List<ApproverDto>>> GetApproversAsync()
  {
    try
    {
      var client = await GetAuthenticatedClientAsync();
      var response = await client.GetAsync("/users/approvers");

      if (response.IsSuccessStatusCode)
      {
        var result = await response.Content.ReadFromJsonAsync<List<ListApproversResponse>>();
        // Map to ApproverDto (IsFavorite will be set by component)
        var approvers = result?.Select(r => new ApproverDto
        {
          Id = r.Id,
          FirstName = r.FirstName,
          LastName = r.LastName,
          Email = r.Email,
          IsFavorite = false
        }).ToList() ?? [];

        return ServiceResult<List<ApproverDto>>.Success(approvers);
      }

      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return ServiceResult<List<ApproverDto>>.Failure(
          response.StatusCode,
          null,
          problemDetails?.Detail
      );
    }
    catch (Exception ex)
    {
      return ServiceResult<List<ApproverDto>>.Failure(
          HttpStatusCode.InternalServerError,
          null,
          ex.Message
      );
    }
  }
}
