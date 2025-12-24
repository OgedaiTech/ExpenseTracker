using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Tenants.Endpoints.Create;
using ExpenseTracker.Tenants.Tests.Abstractions;

namespace ExpenseTracker.Tenants.Tests.Create;

public class CreateIntegrationTests(IntegrationTestWebAppFactory factory) : BaseIntegrationTest(factory)
{
  [Fact]
  public async Task CreateTenantShouldReturnCreatedTenantAsync()
  {
    // Arrange
    await ResetDatabaseAsync();
    var request = new CreateTenantRequest
    {
      Name = "Test Tenant",
      Code = "TT001",
      Description = "This is a test tenant",
      Domain = "testtenant.com",
      Email = "admin@testtenant.com",
      Password = "Password1!"
    };

    // Act
    var response = await Client.PostAsJsonAsync("/tenants", request);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
  }

  [Fact]
  public async Task CreateTenantShouldReturnInternalServerErrorWhenMediatorReturnsErrorAsync()
  {
    await ResetDatabaseAsync();
    Mediator.ShouldReturnError = true;
    var request = new CreateTenantRequest
    {
      Name = "Test Tenant",
      Code = "TT001",
      Description = "This is a test tenant",
      Domain = "testtenant.com",
      Email = "admin@testtenant.com",
      Password = "Password1!"
    };

    var response = await Client.PostAsJsonAsync("/tenants", request);

    Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
    Mediator.ShouldReturnError = false;
  }
}
