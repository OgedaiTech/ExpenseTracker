using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Endpoints;
using ExpenseTracker.Receipts.Endpoints.Create;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExpenseTracker.Receipts.Tests.Create;

public class CreateEndpointTests
{
  private readonly Mock<ICreateReceiptService> _createReceiptService = new();
  private readonly CreateReceiptEndpoint _endpoint;
  public CreateEndpointTests()
  {

    var httpContext = new DefaultHttpContext();
    httpContext.AddTestServices(services =>
    {
      services.AddServicesForUnitTesting();
      services.AddRouting();
      services.AddSingleton(_createReceiptService.Object);
    });

    _endpoint = Factory.Create<CreateReceiptEndpoint>(httpContext);
  }

  [Fact]
  public async Task GivenValidRequest_WhenHandleAsync_ThenShouldCallCreateExpenseServiceAsync()
  {
    // Arrange
    _createReceiptService
        .Setup(s => s.CreateReceiptAsync(It.IsAny<CreateReceiptRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ServiceResult());

    // Act
    await _endpoint.HandleAsync(It.IsAny<CreateReceiptRequest>(), It.IsAny<CancellationToken>());

    // Assert
    _createReceiptService.Verify(
        s => s.CreateReceiptAsync(It.IsAny<CreateReceiptRequest>(), It.IsAny<CancellationToken>()),
        Times.Once);
  }

  [Fact]
  public async Task ShouldReturnBadRequestWhenExpenseModuleCannotAddAmountAsync()
  {
    // Arrange
    _createReceiptService
        .Setup(s => s.CreateReceiptAsync(It.IsAny<CreateReceiptRequest>(), It.IsAny<CancellationToken>()))
        .ReturnsAsync(new ServiceResult(ServiceConstants.CANT_ADD_AMOUNT_TO_EXPENSE));

    // Act
    await _endpoint.HandleAsync(It.IsAny<CreateReceiptRequest>(), It.IsAny<CancellationToken>());

    // Assert
    _createReceiptService.Verify(
        s => s.CreateReceiptAsync(It.IsAny<CreateReceiptRequest>(), It.IsAny<CancellationToken>()),
        Times.Once);
    Assert.Equal(400, _endpoint.HttpContext.Response.StatusCode);
  }
}

