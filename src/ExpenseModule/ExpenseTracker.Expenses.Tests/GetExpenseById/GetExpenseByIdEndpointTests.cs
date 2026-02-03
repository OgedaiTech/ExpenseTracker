using System.Security.Claims;
using ExpenseTracker.Core;
using ExpenseTracker.Expenses.Endpoints.GetExpenseById;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace ExpenseTracker.Expenses.Tests.GetExpenseById;

public class GetExpenseByIdEndpointTests
{
  private readonly Mock<IGetExpenseByIdService> _getExpenseByIdService = new();

  private readonly GetExpenseByIdEndpoint _endpoint;
  private readonly string _userId = "550e8400-e29b-41d4-a716-446655440000";
  private readonly string _tenantId = "660e8400-e29b-41d4-a716-446655440000";
  private readonly string _expenseId = "770e8400-e29b-41d4-a716-446655440000";

  public GetExpenseByIdEndpointTests()
  {
    var logger = NullLogger<GetExpenseByIdEndpoint>.Instance;
    var httpContext = new DefaultHttpContext();
    httpContext.AddTestServices(services =>
    {
      services.AddServicesForUnitTesting();
      services.AddRouting();
      services.AddSingleton(_getExpenseByIdService.Object);
      services.AddSingleton(logger);
    });

    httpContext.Request.RouteValues["id"] = _expenseId;

    var claims = new[] { new Claim("UserId", _userId), new Claim("TenantId", _tenantId) };
    var identity = new ClaimsIdentity(claims, "test");
    httpContext.User = new ClaimsPrincipal(identity);
    _endpoint = Factory.Create<GetExpenseByIdEndpoint>(httpContext);
  }

  [Fact]
  public async Task GivenValidExpenseId_WhenHandleAsync_ThenShouldCallGetExpenseByIdServiceAsync()
  {
    // Arrange
    var expense = new Expense
    {
      Id = Guid.Parse(_expenseId),
      Name = "Test Expense",
      Amount = 100.50m,
      CreatedByUserId = Guid.Parse(_userId),
      TenantId = Guid.Parse(_tenantId),
      CreatedAt = DateTime.UtcNow
    };

    var ct = CancellationToken.None;
    _getExpenseByIdService
        .Setup(s => s.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, ct))
        .ReturnsAsync(new ServiceResult<Expense>(expense));

    // Act
    await _endpoint.HandleAsync(new GetExpenseByIdRequest(null), ct);

    // Assert
    _getExpenseByIdService.Verify(
        s => s.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, ct),
        Times.Once);
    Assert.Equal(200, _endpoint.HttpContext.Response.StatusCode);
  }

  [Fact]
  public async Task GivenNonExistingExpenseId_WhenHandleAsync_ThenShouldReturn404Async()
  {
    // Arrange
    var ct = CancellationToken.None;
    _getExpenseByIdService
        .Setup(s => s.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, ct))
        .ReturnsAsync(new ServiceResult<Expense>("Expense not found."));

    // Act
    await _endpoint.HandleAsync(new GetExpenseByIdRequest(null), ct);

    // Assert
    _getExpenseByIdService.Verify(
        s => s.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, ct),
        Times.Once);
    Assert.Equal(404, _endpoint.HttpContext.Response.StatusCode);
  }

  [Fact]
  public async Task GivenInvalidExpenseId_WhenHandleAsync_ThenShouldReturn400Async()
  {
    // Arrange
    var ct = CancellationToken.None;
    _getExpenseByIdService
        .Setup(s => s.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, ct))
        .ReturnsAsync(new ServiceResult<Expense>("Invalid expense ID."));

    // Act
    await _endpoint.HandleAsync(new GetExpenseByIdRequest(null), ct);

    // Assert
    _getExpenseByIdService.Verify(
        s => s.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, ct),
        Times.Once);
    Assert.Equal(400, _endpoint.HttpContext.Response.StatusCode);
  }
}
