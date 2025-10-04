using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Endpoints;
using ExpenseTracker.Receipts.Endpoints.Create;
using ExpenseTracker.Receipts.UseCases;
using MediatR;
using Moq;

namespace ExpenseTracker.Receipts.Tests.Create;

public class CreateServiceTests
{
  private ICreateReceiptRepository _repository { get; set; }
  private CreateReceiptService _service { get; set; }
  private readonly Mock<IMediator> _mediator = new();
  public CreateServiceTests()
  {
    _repository = Mock.Of<ICreateReceiptRepository>();
    _service = new CreateReceiptService(_repository, _mediator.Object);
  }

  [Fact]
  public async Task ReturnsServiceResultWithValidContentAsync()
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

    _mediator
      .Setup(m => m.Send(It.IsAny<AddAmountToExpenseCommand>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(new ServiceResult<Guid>(It.IsAny<Guid>())));

    // Act
    await _service.CreateReceiptAsync(request, CancellationToken.None);

    // Assert
    Mock.Get(_repository)
      .Verify(r => r.CreateReceiptAsync(request, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task ReturnsServiceResultFailureWhenExpenseModuleFailsAsync()
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

    _mediator
      .Setup(m => m.Send(It.IsAny<AddAmountToExpenseCommand>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(new ServiceResult<Guid>(It.IsAny<string>())));

    // Act
    var result = await _service.CreateReceiptAsync(request, CancellationToken.None);

    // Assert
    Assert.Multiple(() =>
    {
      Assert.False(result.Success);
      Assert.Equal(ServiceConstants.CANT_ADD_AMOUNT_TO_EXPENSE, result.Message);
    });
  }
}

