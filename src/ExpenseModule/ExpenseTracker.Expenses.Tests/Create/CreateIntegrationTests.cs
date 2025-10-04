using System.Net.Http.Json;
using ExpenseTracker.Expenses.Endpoints.Create;
using ExpenseTracker.Expenses.Tests.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Tests.Create;

public class CreateIntegrationTests : BaseIntegrationTest
{
  public CreateIntegrationTests(IntegrationTestWebAppFactory factory)
    : base(factory)
  {

  }

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
