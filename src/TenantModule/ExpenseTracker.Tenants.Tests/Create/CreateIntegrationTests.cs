using System.Net;
using System.Net.Http.Json;
using ExpenseTracker.Tenants.Endpoints.Create;
using ExpenseTracker.Tenants.Tests.Abstractions;

namespace ExpenseTracker.Tenants.Tests.Create;

public class CreateIntegrationTests : BaseIntegrationTest
{
  public CreateIntegrationTests(IntegrationTestWebAppFactory factory)
    : base(factory)
  {

  }

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
    };

    // Act
    var response = await Client.PostAsJsonAsync("/tenants", request);

    // Assert
    Assert.Equal(HttpStatusCode.Created, response.StatusCode);
  }
}
