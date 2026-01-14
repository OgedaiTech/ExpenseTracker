using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Users.Tests.Abstractions;
using ExpenseTracker.Users.UserEndpoints.CreateNewUser;
using ExpenseTracker.Users.UserEndpoints.LoginLogout;

namespace ExpenseTracker.Users.Tests.Login;

public class LoginIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
  [Fact]
  public async Task LoginWithValidCredentialsShouldReturnOkAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var createUserRequest = new CreateUserRequest("test@email.com", "Password123!");

    await Client.PostAsJsonAsync("/users", createUserRequest);
    var userLoginRequest = new UserLoginRequest("test@email.com", "Password123!");

    // Act
    var response = await Client.PostAsJsonAsync("/users/login", userLoginRequest);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);
  }

  [Fact]
  public async Task LoginWithInvalidPasswordShouldReturnUnauthorizedAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var createUserRequest = new CreateUserRequest("test@email.com", "Password123!");

    await Client.PostAsJsonAsync("/users", createUserRequest);
    var userLoginRequest = new UserLoginRequest("test@email.com", "Password123567!");

    // Act
    var response = await Client.PostAsJsonAsync("/users/login", userLoginRequest);

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task LoginWithNonExistingEmailShouldReturnUnauthorizedAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var userLoginRequest = new UserLoginRequest("test@email.com", "Password123!");

    // Act
    var response = await Client.PostAsJsonAsync("/users/login", userLoginRequest);

    // Assert
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }
}
