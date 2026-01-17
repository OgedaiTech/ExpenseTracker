using ExpenseTracker.Core;

namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public interface IListUsersService
{
  Task<ServiceResult<ListUsersServiceResult>> ListUsersAsync(
    Guid tenantId,
    string? cursor,
    int pageSize,
    string? searchQuery,
    bool? isDeactivated,
    CancellationToken ct);
}

public class ListUsersServiceResult
{
  public List<ApplicationUser> Users { get; set; } = [];
  public string? NextCursor { get; set; }
  public bool HasMore { get; set; }
}
