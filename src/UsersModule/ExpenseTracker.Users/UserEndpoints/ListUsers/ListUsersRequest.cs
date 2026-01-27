namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public class ListUsersRequest
{
  public string? Cursor { get; set; }
  public int PageSize { get; set; } = 50;
  public string? SearchQuery { get; set; }
  public bool? IsDeactivated { get; set; }
}
