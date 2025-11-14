using System.Net;
using ExpenseTracker.Receipts.Tests.Abstractions;

namespace ExpenseTracker.Receipts.Tests.ListExpenseReceipts;

public class ListExpenseReceiptIntegrationTests(
  CustomWebApplicationFactory factory)
    : Base2IntegrationTest(factory), IClassFixture<CustomWebApplicationFactory>
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
    SetNoAuthentication();
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
