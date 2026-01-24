namespace ExpenseTrackerUI.Services.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NationalIdentityNo { get; set; }
    public string? TaxIdNo { get; set; }
    public string? EmployeeId { get; set; }
    public string? Title { get; set; }
    public bool IsDeactivated { get; set; }
}

public class ListUsersResponse
{
    public List<UserDto> Users { get; set; } = [];
    public string? NextCursor { get; set; }
    public bool HasMore { get; set; }
    public int PageSize { get; set; }
}
