using ExpenseTracker.Expenses.Endpoints.GetExpenseById;
using Moq;

namespace ExpenseTracker.Expenses.Tests.GetExpenseById;

public class GetExpenseByIdServiceTests
{
    private GetExpenseByIdService _service { get; set; }
    private IGetExpenseByIdRepository _repository { get; set; }
    private readonly string _userId = "550e8400-e29b-41d4-a716-446655440000";
    private readonly string _tenantId = "660e8400-e29b-41d4-a716-446655440000";
    private readonly string _expenseId = "770e8400-e29b-41d4-a716-446655440000";

    public GetExpenseByIdServiceTests()
    {
        _repository = Mock.Of<IGetExpenseByIdRepository>();
        _service = new GetExpenseByIdService(_repository);
    }

    [Fact]
    public async Task ReturnsServiceResultSuccessWhenExpenseExistsAsync()
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

        Mock.Get(_repository)
            .Setup(r => r.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expense);

        // Act
        var result = await _service.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(_expenseId, result.Data.Id.ToString());
            Assert.Equal("Test Expense", result.Data.Name);
            Assert.Equal(100.50m, result.Data.Amount);
        });
    }

    [Fact]
    public async Task ReturnsServiceResultFailureWhenExpenseNotFoundAsync()
    {
        // Arrange
        Mock.Get(_repository)
            .Setup(r => r.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Expense?)null);

        // Act
        var result = await _service.GetExpenseByIdAsync(_expenseId, _userId, _tenantId, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.Success);
            Assert.Equal("Expense not found.", result.Message);
        });
    }

    [Fact]
    public async Task ReturnsServiceResultFailureWhenExpenseIdIsInvalidAsync()
    {
        // Arrange & Act
        var result = await _service.GetExpenseByIdAsync("invalid-id", _userId, _tenantId, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.Success);
            Assert.Equal("Invalid expense ID.", result.Message);
        });

        Mock.Get(_repository)
            .Verify(r => r.GetExpenseByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ReturnsServiceResultFailureWhenExpenseIdIsEmptyAsync()
    {
        // Arrange & Act
        var result = await _service.GetExpenseByIdAsync(string.Empty, _userId, _tenantId, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.Success);
            Assert.Equal("Invalid expense ID.", result.Message);
        });

        Mock.Get(_repository)
            .Verify(r => r.GetExpenseByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task ReturnsServiceResultFailureWhenExpenseIdIsEmptyGuidAsync()
    {
        // Arrange & Act
        var result = await _service.GetExpenseByIdAsync(Guid.Empty.ToString(), _userId, _tenantId, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.False(result.Success);
            Assert.Equal("Invalid expense ID.", result.Message);
        });

        Mock.Get(_repository)
            .Verify(r => r.GetExpenseByIdAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
