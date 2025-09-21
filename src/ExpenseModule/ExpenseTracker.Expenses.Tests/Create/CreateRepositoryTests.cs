using ExpenseTracker.Expenses.Data;
using ExpenseTracker.Expenses.Endpoints.Create;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Tests.Create;

public class CreateRepositoryTests
{
  private readonly ExpenseDbContext _dbContext;
  private readonly CreateExpenseRepository _repository;

  public CreateRepositoryTests()
  {
    _dbContext = ContextGenerator.CreateDbContext();
    _repository = new CreateExpenseRepository(_dbContext);
  }

  [Fact]
  public async Task CreateRepository_WithValidContext_CreatesInstanceAsync()
  {
    // Arrange
    string expenseName = "Sample Expense";

    // Act
    await _repository.CreateExpenseAsync(expenseName, CancellationToken.None);

    // Assert
    var expenses = await _dbContext.Expenses.ToListAsync();
    Assert.Single(expenses);
    var expense = expenses[0];
    Assert.Equal(expenseName, expense.Name);
    Assert.NotEqual(default, expense.CreatedAt);
  }
}
