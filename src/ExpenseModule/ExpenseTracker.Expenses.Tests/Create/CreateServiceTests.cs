using ExpenseTracker.Expenses.Endpoints.Create;
using Moq;

namespace ExpenseTracker.Expenses.Tests.Create;

public class CreateServiceTests
{
  private ICreateExpenseRepository _repository { get; set; }
  private CreateExpenseService _service { get; set; }
  private readonly string _userId = "test-user-id";
  public CreateServiceTests()
  {
    _repository = Mock.Of<ICreateExpenseRepository>();
    _service = new CreateExpenseService(_repository);
  }

  [Fact]
  public async Task CreateService_WithValidRepository_CreatesInstanceAsync()
  {
    // Arrange
    var expenseName = "Sample Expense";
    Mock.Get(_repository)
      .Setup(r => r.CreateExpenseAsync(expenseName, _userId, It.IsAny<CancellationToken>()))
      .Returns(Task.CompletedTask);

    // Act
    await _service.CreateExpenseAsync(expenseName, _userId, CancellationToken.None);

    // Assert
    Mock.Get(_repository)
      .Verify(r => r.CreateExpenseAsync(expenseName, _userId, It.IsAny<CancellationToken>()), Times.Once);
  }

  [Fact]
  public async Task CreateService_WithEmptyName_ReturnsFailureResultAsync()
  {
    // Arrange
    var emptyName = "   ";  // String with only whitespace

    // Act
    var result = await _service.CreateExpenseAsync(emptyName, _userId, CancellationToken.None);

    // Assert
    Assert.Multiple(() =>
    {
      Assert.False(result.Success);
      Assert.Equal("Expense name cannot be empty.", result.Message);
    });
  }

  [Fact]
  public async Task CreateService_WithLongNameThan128_ReturnsFailureResultAsync()
  {
    // Arrange
    var longName = new string('a', 129);  // String with 129 characters

    // Act
    var result = await _service.CreateExpenseAsync(longName, _userId, CancellationToken.None);

    // Assert
    Assert.Multiple(() =>
    {
      Assert.False(result.Success);
      Assert.Equal("Expense name cannot exceed 128 characters.", result.Message);
    });
  }
}
