using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace ExpenseTrackerUI.Services;

public class ExpenseService(IHttpClientFactory httpClientFactory, ProtectedLocalStorage localStorage)
{
  public async Task<ExpenseListResponse?> GetUserExpensesAsync()
  {
    var client = httpClientFactory.CreateClient("AuthenticatedClient");

    // Get the JWT token from browser storage
    var tokenResult = await localStorage.GetAsync<string>("jwt_token");
    if (tokenResult.Success && !string.IsNullOrEmpty(tokenResult.Value))
    {
      client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResult.Value);
    }

    var response = await client.GetAsync("/expenses/users");
    response.EnsureSuccessStatusCode();
    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<ExpenseListResponse>();
    }
    return null;
  }
}

public class ExpenseListResponse
{
  public List<ExpenseDto>? Items { get; set; }
  public int TotalCount { get; set; }
}

public record ExpenseDto(
  Guid Id,
  string Name,
  decimal Amount,
  DateTime CreatedAt
);
