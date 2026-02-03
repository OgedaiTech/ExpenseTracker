using System.Net;
using System.Text.Json.Serialization;
using ExpenseTrackerUI.Services.Authentication;

namespace ExpenseTrackerUI.Services.Approval;

public class ApprovalService(IHttpClientFactory httpClientFactory, CustomAuthStateProvider authStateProvider)
  : AuthenticatedServiceBase(httpClientFactory, authStateProvider)
{
  public async Task<List<PendingApprovalDto>?> GetPendingApprovalsAsync()
  {
    var client = await GetAuthenticatedClientAsync();
    var response = await client.GetAsync("/expenses/pending-approvals");

    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<List<PendingApprovalDto>>();
    }
    return null;
  }

  public async Task<ServiceResult<ApproveExpenseResponseDto?>> ApproveExpenseAsync(Guid expenseId)
  {
    var client = await GetAuthenticatedClientAsync();
    var response = await client.PostAsync($"/expenses/{expenseId}/approve", null);

    if (response.IsSuccessStatusCode)
    {
      var result = await response.Content.ReadFromJsonAsync<ApproveExpenseResponseDto>();
      return ServiceResult<ApproveExpenseResponseDto?>.Success(result);
    }
    else if (response.StatusCode is HttpStatusCode.BadRequest)
    {
      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return ServiceResult<ApproveExpenseResponseDto?>.Failure(
        response.StatusCode,
        "ApprovalError",
        problemDetails?.Detail ?? "An error occurred while approving the expense."
      );
    }
    else
    {
      return ServiceResult<ApproveExpenseResponseDto?>.Failure(
        response.StatusCode,
        "ApprovalError",
        "An error occurred while approving the expense."
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
