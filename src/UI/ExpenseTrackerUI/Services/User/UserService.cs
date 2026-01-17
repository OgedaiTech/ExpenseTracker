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

            // Try to parse ProblemDetails for error information
            try
            {
                var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
                return ServiceResult<CreateUserWithInvitationResponse>.Failure(
                    response.StatusCode,
                    problemDetails?.Detail,
                    null
                );
            }
            catch
            {
                // Fallback if response is not ProblemDetails
                var errorContent = await response.Content.ReadAsStringAsync();
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
