using ExpenseTrackerUI.Services.Authentication;

namespace ExpenseTrackerUI.Services.Receipt;

public class ReceiptService(IHttpClientFactory httpClientFactory, CustomAuthStateProvider authStateProvider)
  : AuthenticatedServiceBase(httpClientFactory, authStateProvider)
{
  public async Task<ReceiptListResponse?> GetReceiptsAsync(Guid expenseId)
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.GetAsync($"/expenses/{expenseId}/receipts");
    response.EnsureSuccessStatusCode();
    if (response.IsSuccessStatusCode)
    {
      return await response.Content.ReadFromJsonAsync<ReceiptListResponse>();
    }
    return null;
  }
}
