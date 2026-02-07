using ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;
using Moq;

namespace ExpenseTracker.Expenses.Tests.ListUsersExpenses;

public class ListUserExpensesServiceTests
{
  private ListUsersExpensesService _service { get; set; }
  private IListUsersExpensesRepository _repository { get; set; }
  private readonly string _userId = "test-user-id";
  private readonly string _tenantId = "test-tenant-id";
  public ListUserExpensesServiceTests()
  {
    _repository = Mock.Of<IListUsersExpensesRepository>();
    _service = new ListUsersExpensesService(_repository);
  }

  [Fact]
  public async Task ReturnsServiceResultFailureWhenRequestingUserAndCreatingUserDoNotMatchAsync()
  {
    // Arrange
    Mock.Get(_repository)
      .Setup(r => r.HasExpenseAsync(_userId, _tenantId, It.IsAny<CancellationToken>()))
      .ReturnsAsync(true);

    Mock.Get(_repository)
      .Setup(r => r.VerifyUserAccessAsync(_userId, _tenantId, It.IsAny<CancellationToken>()))
      .ReturnsAsync(false);

    // Act
    var result = await _service.ListUsersExpensesAsync(_userId, _tenantId, CancellationToken.None);

    // Assert
    Assert.Multiple(() =>
    {
      Assert.False(result.Success);
      Assert.Equal("User does not have access to the requested expenses.", result.Message);
    });
  }
}
