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

  public async Task<ServiceResult> DeleteReceiptAsync(Guid receiptId)
  {
    var client = await GetAuthenticatedClientAsync();

    var response = await client.DeleteAsync($"/receipts/{receiptId}");

    if (response.IsSuccessStatusCode)
    {
      return new ServiceResult();
    }
    else
    {
      var problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetailsResponse>();
      return new ServiceResult(problemDetails?.Detail ?? "An error occurred while deleting the receipt.");
    }
  }
}
