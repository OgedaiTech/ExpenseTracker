using ExpenseTracker.Tenants.Endpoints.Create;
using Moq;

namespace ExpenseTracker.Tenants.Tests.Create;

public class CreateServiceTests
{
  private ICreateTenantRepository _repository { get; set; }
  private CreateTenantService _service { get; set; }
  public CreateServiceTests()
  {
    _repository = Mock.Of<ICreateTenantRepository>();
    _service = new CreateTenantService(_repository);
  }

  [Fact]
  public async Task ReturnsServiceResultWithValidContentAsync()
  {
    // Arrange
    var request = new CreateTenantRequest
    {
      Name = "Test Tenant",
      Code = "TT001",
      Description = "This is a test tenant",
      Domain = "testtenant.com",
    };
    Mock.Get(_repository)
      .Setup(r => r.CreateTenantAsync(request, It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(Guid.Empty));

    // Act
    await _service.CreateTenantAsync(request, CancellationToken.None);

    // Assert
    Mock.Get(_repository)
      .Verify(r => r.CreateTenantAsync(request, It.IsAny<CancellationToken>()), Times.Once);
  }
}
