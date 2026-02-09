using System.Net;
using ExpenseTrackerUI.Services.Authentication;

namespace ExpenseTrackerUI.Services.Expense;

public class ExpenseService(IHttpClientFactory httpClientFactory, CustomAuthStateProvider authStateProvider)
  : AuthenticatedServiceBase(httpClientFactory, authStateProvider)
{
  public async Task<ExpenseListResponse?> GetUserExpensesAsync()
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.GetAsync("/expenses/users");
    response.EnsureSuccessStatusCode();
    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<ExpenseListResponse>();
    }
    return null;
  }

  public async Task<ServiceResult<ExpenseDto?>> GetExpenseByIdAsync(Guid expenseId, Guid? createdByUserId)
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.GetAsync($"/expenses/{expenseId}?createdByUserId={createdByUserId}");
    if (response.IsSuccessStatusCode)
    {
      var result = await response.Content.ReadFromJsonAsync<GetExpenseByIdResponse>();
      return ServiceResult<ExpenseDto?>.Success(result?.Expense);
    }
    else if (response.StatusCode is HttpStatusCode.NotFound)
    {
      return ServiceResult<ExpenseDto?>.Failure(HttpStatusCode.NotFound, "ExpenseNotFound", "The requested expense was not found.");
    }
    else
    {
      return ServiceResult<ExpenseDto?>.Failure(response.StatusCode, "ExpenseRetrievalError", "An error occurred while retrieving the expense.");
    }
  }

  public async Task<ServiceResult> DeleteExpenseAsync(Guid expenseId)
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.DeleteAsync($"/expenses/{expenseId}");
    if (response.IsSuccessStatusCode)
    {
      return new ServiceResult();
    }
    else if (response.StatusCode is HttpStatusCode.BadRequest)
    {
      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return new ServiceResult(problemDetails?.Detail ?? "An error occurred while deleting the expense.");
    }
    else
    {
      return new ServiceResult("An error occurred while deleting the expense.");
    }
  }

  public async Task<ServiceResult<SubmitExpenseResponse?>> SubmitExpenseAsync(Guid expenseId, SubmitExpenseRequest request)
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.PostAsJsonAsync($"/expenses/{expenseId}/submit", request);
    if (response.IsSuccessStatusCode)
    {
      var result = await response.Content.ReadFromJsonAsync<SubmitExpenseResponse>();
      return ServiceResult<SubmitExpenseResponse?>.Success(result);
    }
    else if (response.StatusCode is HttpStatusCode.BadRequest)
    {
      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return ServiceResult<SubmitExpenseResponse?>.Failure(response.StatusCode, "ExpenseSubmissionError", problemDetails?.Detail ?? "An error occurred while submitting the expense.");
    }
    else
    {
      return ServiceResult<SubmitExpenseResponse?>.Failure(response.StatusCode, "ExpenseSubmissionError", "An error occurred while submitting the expense.");
    }
  }

  public async Task<ServiceResult<UpdateExpenseResponse?>> UpdateExpenseAsync(Guid expenseId, UpdateExpenseDto request)
  {
    var client = await GetAuthenticatedClientAsync();

    var backendRequest = new { Name = request.Name };
    var response = await client.PutAsJsonAsync($"/expenses/{expenseId}", backendRequest);
    if (response.IsSuccessStatusCode)
    {
      var result = await response.Content.ReadFromJsonAsync<UpdateExpenseResponse>();
      return ServiceResult<UpdateExpenseResponse?>.Success(result);
    }
    else if (response.StatusCode is HttpStatusCode.BadRequest)
    {
      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return ServiceResult<UpdateExpenseResponse?>.Failure(response.StatusCode, "UpdateExpenseError", problemDetails?.Detail ?? "An error occurred while updating the expense.");
    }
    else
    {
      return ServiceResult<UpdateExpenseResponse?>.Failure(response.StatusCode, "UpdateExpenseError", "An error occurred while updating the expense.");
    }
  }

  private sealed record GetExpenseByIdResponse(ExpenseDto Expense);
}
