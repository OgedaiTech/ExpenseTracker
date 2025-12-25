using System.Security.Claims;
using ExpenseTracker.Core;
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
  private readonly string _userId = "test-user-id";
  private readonly string _tenantId = "test-tenant-id";
  public CreateEndpointTests()
  {

    var httpContext = new DefaultHttpContext();
    httpContext.AddTestServices(services =>
    {
      services.AddServicesForUnitTesting();
      services.AddRouting();
      services.AddSingleton(_createExpenseService.Object);
    });

    var claims = new[] { new Claim("UserId", _userId), new Claim("TenantId", _tenantId) };
    var identity = new ClaimsIdentity(claims, "test");
    httpContext.User = new ClaimsPrincipal(identity);
    _endpoint = Factory.Create<CreateExpenseEndpoint>(httpContext);
  }

  [Fact]
  public async Task GivenValidRequest_WhenHandleAsync_ThenShouldCallCreateExpenseServiceAsync()
  {
    // Arrange
    var request = new CreateExpenseRequest { Name = "Test Expense" };
    var ct = CancellationToken.None;
    _createExpenseService
        .Setup(s => s.CreateExpenseAsync("Test Expense", _userId, _tenantId, ct))
        .ReturnsAsync(new ServiceResult());

    // Act
    await _endpoint.HandleAsync(request, ct);

    // Assert
    _createExpenseService.Verify(
        s => s.CreateExpenseAsync("Test Expense", _userId, _tenantId, ct),
        Times.Once);
  }

  [Fact]
  public async Task GivenInvalidRequest_WhenHandleAsync_ThenShouldReturnBadRequestAsync()
  {
    // Arrange
    var request = new CreateExpenseRequest { Name = "" };
    var ct = CancellationToken.None;
    _createExpenseService
        .Setup(s => s.CreateExpenseAsync("", _userId, _tenantId, ct))
        .ReturnsAsync(new ServiceResult("Expense name cannot be empty."));

    // Act
    await _endpoint.HandleAsync(request, ct);

    // Assert
    _createExpenseService.Verify(
        s => s.CreateExpenseAsync("", _userId, _tenantId, ct),
        Times.Once);
    Assert.Equal(400, _endpoint.HttpContext.Response.StatusCode);
  }
}
