using System.Net.Http.Headers;

namespace ExpenseTrackerUI.Services;

public class ExpenseService(IHttpClientFactory httpClientFactory)
{
  public async Task<ExpenseListResponse?> GetUserExpensesAsync(string token)
  {
    var client = httpClientFactory.CreateClient("AuthenticatedClient");
    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
