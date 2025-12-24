using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Users.Tests.Abstractions;
using ExpenseTracker.Users.UserEndpoints;

namespace ExpenseTracker.Users.Tests.Create;

public class CreateIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
  [Fact]
  public async Task CreateUserShouldReturnOkAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var request = new CreateUserRequest("test@email.com", "Password123!", Guid.NewGuid());

    // Act
    var response = await Client.PostAsJsonAsync("/users", request);

    // Assert
    Assert.Multiple(() =>
    {
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      var createdUser = DbContext.Users.First();
      Assert.Equal(request.Email, createdUser.Email);
    });
  }
}
