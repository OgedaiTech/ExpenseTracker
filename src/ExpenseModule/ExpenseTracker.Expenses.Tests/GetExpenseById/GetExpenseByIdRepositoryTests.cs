using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Expenses.Endpoints.GetExpenseById;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Tests.GetExpenseById;

public class GetExpenseByIdRepositoryTests
{
    private readonly ExpenseDbContext _dbContext;
    private readonly GetExpenseByIdRepository _repository;
    private readonly Guid _userId = Guid.Parse("550e8400-e29b-41d4-a716-446655440000");
    private readonly Guid _tenantId = Guid.Parse("660e8400-e29b-41d4-a716-446655440000");
    private readonly Guid _expenseId = Guid.Parse("770e8400-e29b-41d4-a716-446655440000");

    public GetExpenseByIdRepositoryTests()
    {
        _dbContext = ContextGenerator.CreateDbContext();
        _repository = new GetExpenseByIdRepository(_dbContext);
    }

    [Fact]
    public async Task GetExpenseByIdRepository_WithExistingExpense_ReturnsExpenseAsync()
    {
        // Arrange
        var expense = new Expense
        {
            Id = _expenseId,
            Name = "Test Expense",
            Amount = 100.50m,
            CreatedByUserId = _userId,
            TenantId = _tenantId,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();

        // Act
        var result = await _repository.GetExpenseByIdAsync(_expenseId.ToString(), _userId.ToString(), _tenantId.ToString(), CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.NotNull(result);
            Assert.Equal(_expenseId, result.Id);
            Assert.Equal("Test Expense", result.Name);
            Assert.Equal(100.50m, result.Amount);
        });
    }

    [Fact]
    public async Task GetExpenseByIdRepository_WithNonExistingExpense_ReturnsNullAsync()
    {
        // Arrange
        var nonExistingExpenseId = Guid.Parse("880e8400-e29b-41d4-a716-446655440000");

        // Act
        var result = await _repository.GetExpenseByIdAsync(nonExistingExpenseId.ToString(), _userId.ToString(), _tenantId.ToString(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetExpenseByIdRepository_WithWrongUserId_ReturnsNullAsync()
    {
        // Arrange
        var expense = new Expense
        {
            Id = _expenseId,
            Name = "Test Expense",
            Amount = 100.50m,
            CreatedByUserId = _userId,
            TenantId = _tenantId,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();

        var wrongUserId = Guid.Parse("990e8400-e29b-41d4-a716-446655440000");

        // Act
        var result = await _repository.GetExpenseByIdAsync(_expenseId.ToString(), wrongUserId.ToString(), _tenantId.ToString(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetExpenseByIdRepository_WithWrongTenantId_ReturnsNullAsync()
    {
        // Arrange
        var expense = new Expense
        {
            Id = _expenseId,
            Name = "Test Expense",
            Amount = 100.50m,
            CreatedByUserId = _userId,
            TenantId = _tenantId,
            CreatedAt = DateTime.UtcNow
        };

        await _dbContext.Expenses.AddAsync(expense);
        await _dbContext.SaveChangesAsync();

        var wrongTenantId = Guid.Parse("aa0e8400-e29b-41d4-a716-446655440000");

        // Act
        var result = await _repository.GetExpenseByIdAsync(_expenseId.ToString(), _userId.ToString(), wrongTenantId.ToString(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
