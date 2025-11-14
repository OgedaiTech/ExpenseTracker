using System.Net;
using ExpenseTracker.Receipts.Tests.Abstractions;
using ExpenseTracker.WebAPI;

namespace ExpenseTracker.Receipts.Tests.ListExpenseReceipts;

public class ListExpenseReceiptIntegrationTests
  : IClassFixture<CustomWebApplicationFactory<Program>>
{
  private readonly CustomWebApplicationFactory<Program> _factory;
  private readonly HttpClient _client;

  public ListExpenseReceiptIntegrationTests(
    CustomWebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _client = _factory.CreateClient();
  }

  [Fact]
  public async Task ReturnsOkWithReceiptsWhenUserAuthenticatedAsync()
  {
    // Arrange

    var expenseId = Guid.NewGuid();
    var request = new HttpRequestMessage(
      HttpMethod.Get,
      $"/expenses/{expenseId}/receipts");

    // Act
    var response = await _client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task ReturnsUnauthorizedIfUserNotAuthenticatedAsync()
  {
    // Arrange
    _client.DefaultRequestHeaders.Add("No-Auth", "true");
    var expenseId = Guid.NewGuid();
    var request = new HttpRequestMessage(
      HttpMethod.Get,
      $"/expenses/{expenseId}/receipts");

    // Act
    var response = await _client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}
