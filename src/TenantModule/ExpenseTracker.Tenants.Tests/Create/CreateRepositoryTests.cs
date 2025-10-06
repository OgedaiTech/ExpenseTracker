using ExpenseTracker.Tenants.Data;
using ExpenseTracker.Tenants.Endpoints.Create;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ExpenseTracker.Tenants.Tests.Create;

public class CreateRepositoryTests
{
  private readonly TenantDbContext _dbContext;
  private readonly CreateTenantRepository _repository;

  public CreateRepositoryTests()
  {
    _dbContext = ContextGenerator.CreateDbContext();
    _repository = new CreateTenantRepository(_dbContext);
  }

  [Fact]
  public async Task CreatesTenantWithGivenValidRequestAsync()
  {
    // Arrange
    var request = new CreateTenantRequest
    {
      Name = "Test Tenant",
      Code = "TT001",
      Description = "This is a test tenant",
      Domain = "testtenant.com",
    };

    // Act
    await _repository.CreateTenantAsync(request, It.IsAny<CancellationToken>());

    // Assert
    Assert.Multiple(async () =>
    {
      var tenants = await _dbContext.Tenants.ToListAsync();
      Assert.Single(tenants);
      var tenant = tenants[0];
      Assert.Equal(request.Code, tenant.Code);
    });
  }
}
