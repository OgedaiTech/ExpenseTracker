using System.Net;
using ExpenseTracker.Receipts.Tests.Abstractions;

namespace ExpenseTracker.Receipts.Tests.ListExpenseReceipts;

public class ListExpenseReceiptIntegrationTests(
  IntegrationTestWebAppFactory factory)
    : BaseIntegrationTest(factory), IClassFixture<IntegrationTestWebAppFactory>
{
  [Fact]
  public async Task ReturnsOkWithReceiptsAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var expenseId = Guid.NewGuid();
    DbContext.Receipts.Add(new()
    {
      Id = Guid.NewGuid(),
      ExpenseId = expenseId,
      Amount = 50,
      ReceiptNo = "R-50",
      Vendor = "Vendor A",
      Date = DateTime.UtcNow
    });
    await DbContext.SaveChangesAsync();

    // Act
    var response = await Client.GetAsync($"/expenses/{expenseId}/receipts");

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task ReturnsUnauthorizedIfUserNotAuthenticatedAsync()
  {
    // Arrange
    SetNoAuthentication();
    var expenseId = Guid.NewGuid();

    // Act
    var response = await Client.GetAsync($"/expenses/{expenseId}/receipts");

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}
