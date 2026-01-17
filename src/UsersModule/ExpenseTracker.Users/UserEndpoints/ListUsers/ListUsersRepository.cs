using ExpenseTracker.Users.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public class ListUsersRepository(UsersDbContext context) : IListUsersRepository
{
  public async Task<List<ApplicationUser>> GetUsersPageAsync(
    Guid tenantId,
    UserCursor? cursor,
    int pageSize,
    string? searchQuery,
    bool? isDeactivated,
    CancellationToken ct)
  {
    var hasSearch = !string.IsNullOrWhiteSpace(searchQuery);
    var query = context.Users.OfType<ApplicationUser>().Where(u => u.TenantId == tenantId);

    // Filter by IsDeactivated
    if (isDeactivated.HasValue)
    {
      query = query.Where(u => u.IsDeactivated == isDeactivated.Value);
    }

    // Filter by search query (FirstName OR LastName OR Email)
    if (hasSearch)
    {
      var searchTerm = searchQuery!.Trim().ToLower();
      query = query.Where(u =>
        (u.FirstName != null && u.FirstName.ToLower().Contains(searchTerm)) ||
        (u.LastName != null && u.LastName.ToLower().Contains(searchTerm)) ||
        (u.Email != null && u.Email.ToLower().Contains(searchTerm))
      );
    }

    // Apply cursor filtering
    if (cursor != null)
    {
      query = ApplyCursorFilter(query, cursor, hasSearch);
    }

    // Apply sorting
    query = ApplySorting(query, hasSearch);

    // Fetch pageSize + 1 to determine HasMore
    return await query.Take(pageSize + 1).ToListAsync(ct);
  }

  private static IQueryable<ApplicationUser> ApplyCursorFilter(
    IQueryable<ApplicationUser> query,
    UserCursor cursor,
    bool hasSearch)
  {
    if (hasSearch)
    {
      var cursorFirstName = cursor.FirstName ?? "";
      var cursorLastName = cursor.LastName ?? "";
      var cursorEmail = cursor.Email ?? "";
      var cursorId = cursor.Id;

      return query.Where(u =>
        (u.FirstName ?? "").CompareTo(cursorFirstName) > 0 ||
        ((u.FirstName ?? "") == cursorFirstName && (u.LastName ?? "").CompareTo(cursorLastName) > 0) ||
        ((u.FirstName ?? "") == cursorFirstName && (u.LastName ?? "") == cursorLastName && (u.Email ?? "").CompareTo(cursorEmail) > 0) ||
        ((u.FirstName ?? "") == cursorFirstName && (u.LastName ?? "") == cursorLastName && (u.Email ?? "") == cursorEmail && u.Id.CompareTo(cursorId) > 0)
      );
    }
    else
    {
      var cursorEmail = cursor.Email ?? "";
      var cursorId = cursor.Id;

      return query.Where(u =>
        (u.Email ?? "").CompareTo(cursorEmail) > 0 ||
        ((u.Email ?? "") == cursorEmail && u.Id.CompareTo(cursorId) > 0)
      );
    }
  }

  private static IQueryable<ApplicationUser> ApplySorting(
    IQueryable<ApplicationUser> query,
    bool hasSearch)
  {
    if (hasSearch)
    {
      // Use null coalescing to ensure nulls sort as empty strings (first in alphabetical order)
      return query
        .OrderBy(u => u.FirstName ?? "")
        .ThenBy(u => u.LastName ?? "")
        .ThenBy(u => u.Email ?? "")
        .ThenBy(u => u.Id);
    }
    else
    {
      return query
        .OrderBy(u => u.Email ?? "")
        .ThenBy(u => u.Id);
    }
  }
}
