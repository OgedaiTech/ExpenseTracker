using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Receipts.Endpoints.Create;
using ExpenseTracker.Receipts.Tests.Abstractions;

namespace ExpenseTracker.Receipts.Tests.Create;

public class CreateIntegrationTests(
  IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory), IClassFixture<IntegrationTestWebAppFactory>
{
  [Fact]
  public async Task CreateReceiptShouldReturnCreatedReceiptAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var request = new CreateReceiptRequest
    {
      ExpenseId = Guid.Empty,
      Amount = 100,
      ReceiptNo = "R-100",
      Vendor = "Test Vendor",
      Date = DateTime.UtcNow
    };

    // Act
    var response = await Client.PostAsJsonAsync("/receipts", request);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
  }
}
