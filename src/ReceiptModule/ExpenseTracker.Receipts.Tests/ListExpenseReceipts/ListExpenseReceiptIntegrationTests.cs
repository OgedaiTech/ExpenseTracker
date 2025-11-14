using System.Net;
using ExpenseTracker.Receipts.Tests.Abstractions;
using ExpenseTracker.WebAPI;

namespace ExpenseTracker.Receipts.Tests.ListExpenseReceipts;

public class ListExpenseReceiptIntegrationTests(
  CustomWebApplicationFactory<Program> factory)
    : Base2IntegrationTest(factory), IClassFixture<CustomWebApplicationFactory<Program>>
{
  [Fact]
  public async Task ReturnsOkWithReceiptsWhenUserAuthenticatedAsync()
  {
    // Arrange
    var expenseId = Guid.NewGuid();
    var request = new HttpRequestMessage(
      HttpMethod.Get,
      $"/expenses/{expenseId}/receipts");

    // Act
    var response = await Client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task ReturnsUnauthorizedIfUserNotAuthenticatedAsync()
  {
    // Arrange
    Client.DefaultRequestHeaders.Add("No-Auth", "true");
    var expenseId = Guid.NewGuid();
    var request = new HttpRequestMessage(
      HttpMethod.Get,
      $"/expenses/{expenseId}/receipts");

    // Act
    var response = await Client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}
