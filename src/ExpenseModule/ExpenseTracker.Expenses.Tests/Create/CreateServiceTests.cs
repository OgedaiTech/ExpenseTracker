using ExpenseTracker.Expenses.Endpoints.Create;
using Moq;

namespace ExpenseTracker.Expenses.Tests.Create;

public class CreateServiceTests
{
  private ICreateExpenseRepository _repository { get; set; }
  private CreateExpenseService _service { get; set; }
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
      .Setup(r => r.CreateExpenseAsync(expenseName, It.IsAny<CancellationToken>()))
      .Returns(Task.CompletedTask);

    // Act
    await _service.CreateExpenseAsync(expenseName, CancellationToken.None);

    // Assert
    Mock.Get(_repository)
      .Verify(r => r.CreateExpenseAsync(expenseName, It.IsAny<CancellationToken>()), Times.Once);
  }
}
