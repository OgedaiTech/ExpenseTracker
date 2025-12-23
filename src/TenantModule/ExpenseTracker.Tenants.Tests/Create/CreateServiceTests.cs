using ExpenseTracker.Core;
using ExpenseTracker.Tenants.Endpoints.Create;
using MediatR;
using Moq;

namespace ExpenseTracker.Tenants.Tests.Create;

public class CreateServiceTests
{
  private ICreateTenantRepository _repository { get; set; }
  private CreateTenantService _service { get; set; }
  private readonly Mock<IMediator> _mediator = new();
  public CreateServiceTests()
  {
    _repository = Mock.Of<ICreateTenantRepository>();
    _service = new CreateTenantService(_repository, _mediator.Object);
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
      Email = "admin@testtenant.com",
      Password = "Password1!"
    };
    Mock.Get(_repository)
      .Setup(r => r.CreateTenantAsync(request, It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(Guid.Empty));

    _mediator
      .Setup(m => m.Send(It.IsAny<CreateTenantAdminUserCommand>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(new ServiceResult()));

    // Act
    await _service.CreateTenantAsync(request, CancellationToken.None);

    // Assert
    Mock.Get(_repository)
      .Verify(r => r.CreateTenantAsync(request, It.IsAny<CancellationToken>()), Times.Once);
  }
}
