namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public class UserCursor
{
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string Email { get; set; } = string.Empty;
  public string Id { get; set; } = string.Empty;
}
