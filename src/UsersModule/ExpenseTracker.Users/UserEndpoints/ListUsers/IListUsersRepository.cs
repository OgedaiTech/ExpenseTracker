namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public interface IListUsersRepository
{
  Task<List<ApplicationUser>> GetUsersPageAsync(
    Guid tenantId,
    UserCursor? cursor,
    int pageSize,
    string? searchQuery,
    bool? isDeactivated,
    CancellationToken ct);
}
