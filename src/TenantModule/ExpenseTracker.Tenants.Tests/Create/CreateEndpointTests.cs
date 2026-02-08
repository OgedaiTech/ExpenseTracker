using ExpenseTracker.Core;
using ExpenseTracker.Tenants.Endpoints.Create;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExpenseTracker.Tenants.Tests.Create;

public class CreateEndpointTests
{
  private readonly Mock<ICreateTenantService> _createTenantService = new();
  private readonly CreateTenantEndpoint _endpoint;
  public CreateEndpointTests()
  {
    var httpContext = new DefaultHttpContext();
    httpContext.AddTestServices(services =>
    {
      services.AddServicesForUnitTesting();
      services.AddRouting();
      services.AddSingleton(_createTenantService.Object);
    });

    _endpoint = Factory.Create<CreateTenantEndpoint>(httpContext);
  }

  [Fact]
  public async Task ShouldCallCreateExpenseServiceWithGivenValidRequestAsync()
  {
    // Arrange
    var request = new CreateTenantRequest { Name = "Test Tenant", Email = "test@example.com", Password = "TestPassword123!" };
    _createTenantService
        .Setup(s => s.CreateTenantAsync(It.IsAny<CreateTenantRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ServiceResult<Guid>(Guid.Empty));

    // Act
    await _endpoint.HandleAsync(request, It.IsAny<CancellationToken>());

    // Assert
    _createTenantService.Verify(
        s => s.CreateTenantAsync(request, It.IsAny<CancellationToken>()),
        Times.Once);
  }
}
