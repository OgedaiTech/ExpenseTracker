using ExpenseTracker.Receipts.Endpoints.Create;
using Moq;

namespace ExpenseTracker.Receipts.Tests.Create;

public class CreateServiceTests
{
  private ICreateReceiptRepository _repository { get; set; }
  private CreateReceiptService _service { get; set; }
  public CreateServiceTests()
  {
    _repository = Mock.Of<ICreateReceiptRepository>();
    _service = new CreateReceiptService(_repository);
  }

  [Fact]
  public async Task CreateService_WithValidRepository_CreatesInstanceAsync()
  {
    // Arrange
    var request = new CreateReceiptRequest
    {
      ExpenseId = Guid.Empty,
      Amount = 100,
      ReceiptNo = "R-100",
      Vendor = "Test Vendor",
      Date = DateTime.UtcNow
    };
    Mock.Get(_repository)
      .Setup(r => r.CreateReceiptAsync(request, It.IsAny<CancellationToken>()))
      .Returns(Task.CompletedTask);

    // Act
    await _service.CreateReceiptAsync(request, CancellationToken.None);

    // Assert
    Mock.Get(_repository)
      .Verify(r => r.CreateReceiptAsync(request, It.IsAny<CancellationToken>()), Times.Once);
  }
}

