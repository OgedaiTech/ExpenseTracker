using ExpenseTracker.Expenses.Endpoints.Create;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ExpenseTracker.Expenses.Tests.Create;

public class CreateEndpointTests
{
  private readonly Mock<ICreateExpenseService> _createExpenseService = new();
  private readonly CreateExpenseEndpoint _endpoint;
  public CreateEndpointTests()
  {

    var httpContext = new DefaultHttpContext();
    httpContext.AddTestServices(services =>
    {
      services.AddServicesForUnitTesting();
      services.AddRouting();
      services.AddSingleton(_createExpenseService.Object);
    });

    _endpoint = Factory.Create<CreateExpenseEndpoint>(httpContext);
  }

  [Fact]
  public async Task GivenValidRequest_WhenHandleAsync_ThenShouldCallCreateExpenseServiceAsync()
  {
    // Arrange
    var request = new CreateExpenseRequest { Name = "Test Expense" };
    var ct = CancellationToken.None;
    _createExpenseService
        .Setup(s => s.CreateExpenseAsync("Test Expense", ct))
        .Returns(Task.CompletedTask);

    // Act
    await _endpoint.HandleAsync(request, ct);

    // Assert
    _createExpenseService.Verify(
        s => s.CreateExpenseAsync("Test Expense", ct),
        Times.Once);
  }
}
