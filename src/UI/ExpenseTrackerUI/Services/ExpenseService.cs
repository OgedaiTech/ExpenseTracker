namespace ExpenseTrackerUI.Services;

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
