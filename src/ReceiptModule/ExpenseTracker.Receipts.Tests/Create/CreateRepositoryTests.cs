using ExpenseTracker.Receipts.Data;
using ExpenseTracker.Receipts.Endpoints.Create;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpenseTracker.Receipts.Tests.Create;

public class CreateRepositoryTests
{
  private readonly ReceiptDbContext _dbContext;
  private readonly CreateReceiptRepository _repository;

  public CreateRepositoryTests()
  {
    _dbContext = ContextGenerator.CreateDbContext();
    _repository = new CreateReceiptRepository(_dbContext);
  }

  [Fact]
  public async Task CreateRepository_WithValidContext_CreatesInstanceAsync()
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

    // Act
    await _repository.CreateReceiptAsync(request, It.IsAny<CancellationToken>());

    // Assert
    Assert.Multiple(async () =>
    {
      var receipts = await _dbContext.Receipts.ToListAsync();
      Assert.Single(receipts);
      var receipt = receipts[0];
      Assert.Equal(request.ReceiptNo, receipt.ReceiptNo);
    });
  }
}
