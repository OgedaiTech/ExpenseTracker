using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Endpoints.Create;
using ExpenseTracker.Receipts.UseCases;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExpenseTracker.Receipts.Tests.Create;

public class CreateEndpointTests
{
  private readonly Mock<ICreateReceiptService> _createReceiptService = new();
  private readonly Mock<IMediator> _mediator = new();
  private readonly CreateReceiptEndpoint _endpoint;
  public CreateEndpointTests()
  {

    var httpContext = new DefaultHttpContext();
    httpContext.AddTestServices(services =>
    {
      services.AddServicesForUnitTesting();
      services.AddRouting();
      services.AddSingleton(_createReceiptService.Object);
      services.AddSingleton(_mediator.Object);
    });

    _endpoint = Factory.Create<CreateReceiptEndpoint>(httpContext);
  }

  [Fact]
  public async Task GivenValidRequest_WhenHandleAsync_ThenShouldCallCreateExpenseServiceAsync()
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

    _createReceiptService
        .Setup(s => s.CreateReceiptAsync(request, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ServiceResult());
    _mediator
      .Setup(m => m.Send(It.IsAny<AddAmountToExpenseCommand>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(new ServiceResult<Guid>(Guid.Empty)));

    // Act
    await _endpoint.HandleAsync(request, It.IsAny<CancellationToken>());

    // Assert
    _createReceiptService.Verify(
        s => s.CreateReceiptAsync(request, It.IsAny<CancellationToken>()),
        Times.Once);
  }

  [Fact]
  public async Task GivenInvalidRequest_WhenHandleAsync_ThenShouldReturnBadRequestAsync()
  {
    // Arrange
    var request = new CreateReceiptRequest
    {
      ExpenseId = Guid.Empty,
      Amount = 100,
      ReceiptNo = "",
      Vendor = "Test Vendor",
      Date = DateTime.UtcNow
    };

    _createReceiptService
        .Setup(s => s.CreateReceiptAsync(request, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ServiceResult("Receipt No cannot be empty."));

    // Act
    await _endpoint.HandleAsync(request, It.IsAny<CancellationToken>());

    // Assert
    _createReceiptService.Verify(
        s => s.CreateReceiptAsync(request, It.IsAny<CancellationToken>()),
        Times.Once);
    Assert.Equal(400, _endpoint.HttpContext.Response.StatusCode);
  }

  [Fact]
  public async Task ReturnsBadRequestWhenMediatorFailsAsync()
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

    _createReceiptService
        .Setup(s => s.CreateReceiptAsync(request, It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ServiceResult());
    _mediator
      .Setup(m => m.Send(It.IsAny<AddAmountToExpenseCommand>(), It.IsAny<CancellationToken>()))
      .Returns(Task.FromResult(new ServiceResult<Guid>("Failure Message")));

    // Act
    await _endpoint.HandleAsync(request, It.IsAny<CancellationToken>());

    // Assert
    Assert.Multiple(() =>
    {
      _createReceiptService.Verify(
        s => s.CreateReceiptAsync(request, It.IsAny<CancellationToken>()),
        Times.Once);
      Assert.Equal(400, _endpoint.HttpContext.Response.StatusCode);
    });
  }
}

