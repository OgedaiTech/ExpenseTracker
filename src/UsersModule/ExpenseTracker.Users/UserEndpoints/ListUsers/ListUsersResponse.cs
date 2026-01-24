namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public class ListUsersResponse
{
    public List<UserDto> Users { get; set; } = [];
    public string? NextCursor { get; set; }
    public bool HasMore { get; set; }
    public int PageSize { get; set; }
}
