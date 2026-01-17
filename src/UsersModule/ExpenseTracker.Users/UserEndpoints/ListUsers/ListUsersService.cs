using System.Text;
using System.Text.Json;
using ExpenseTracker.Core;

namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public class ListUsersService(IListUsersRepository repository) : IListUsersService
{
  private const int MinPageSize = 1;
  private const int MaxPageSize = 100;

  public async Task<ServiceResult<ListUsersServiceResult>> ListUsersAsync(
    Guid tenantId,
    string? cursor,
    int pageSize,
    string? searchQuery,
    bool? isDeactivated,
    CancellationToken ct)
  {
    // Validate and clamp page size
    pageSize = Math.Clamp(pageSize, MinPageSize, MaxPageSize);

    // Decode cursor
    var decodedCursor = DecodeCursor(cursor);

    // Trim search query
    var trimmedSearchQuery = string.IsNullOrWhiteSpace(searchQuery)
      ? null
      : searchQuery.Trim();

    // Call repository
    var users = await repository.GetUsersPageAsync(
      tenantId,
      decodedCursor,
      pageSize,
      trimmedSearchQuery,
      isDeactivated,
      ct);

    // Check if has more
    var hasMore = users.Count > pageSize;
    if (hasMore)
    {
      users = users.Take(pageSize).ToList();
    }

    // Encode next cursor
    var nextCursor = hasMore
      ? EncodeCursor(users.LastOrDefault(), !string.IsNullOrWhiteSpace(trimmedSearchQuery))
      : null;

    var result = new ListUsersServiceResult
    {
      Users = users,
      NextCursor = nextCursor,
      HasMore = hasMore
    };

    return new ServiceResult<ListUsersServiceResult>(result);
  }

  private static UserCursor? DecodeCursor(string? cursor)
  {
    if (string.IsNullOrWhiteSpace(cursor))
      return null;

    try
    {
      var json = Encoding.UTF8.GetString(Convert.FromBase64String(cursor));
      return JsonSerializer.Deserialize<UserCursor>(json);
    }
    catch
    {
      return null;
    }
  }

  private static string? EncodeCursor(ApplicationUser? lastUser, bool hasSearchQuery)
  {
    if (lastUser == null)
      return null;

    var cursor = new UserCursor
    {
      FirstName = hasSearchQuery ? lastUser.FirstName : null,
      LastName = hasSearchQuery ? lastUser.LastName : null,
      Email = lastUser.Email ?? string.Empty,
      Id = lastUser.Id
    };

    var json = JsonSerializer.Serialize(cursor);
    return Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
  }
}
