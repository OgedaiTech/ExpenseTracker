using System.Net.Http.Json;
using ExpenseTracker.Expenses.Endpoints.Create;
using ExpenseTracker.Expenses.Tests.Abstractions;

namespace ExpenseTracker.Expenses.Tests.Create;

public class CreateIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
  [Fact]
  public async Task CreateExpenseShouldReturnCreatedExpenseAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var request = new CreateExpenseRequest
    {
      Name = "Test Expense",
    };

    // Act
    var response = await Client.PostAsJsonAsync("/expenses", request);

    // Assert
    Assert.Equal(System.Net.HttpStatusCode.Created, response.StatusCode);
  }
}
