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

  public async Task<ServiceResult<ExpenseDto?>> GetExpenseByIdAsync(Guid expenseId)
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.GetAsync($"/expenses/{expenseId}");
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

  private sealed record GetExpenseByIdResponse(ExpenseDto Expense);
}
