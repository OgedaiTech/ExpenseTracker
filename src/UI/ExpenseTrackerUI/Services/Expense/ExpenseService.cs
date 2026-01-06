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
}
