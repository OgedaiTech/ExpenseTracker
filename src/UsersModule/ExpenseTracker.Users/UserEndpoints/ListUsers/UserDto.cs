namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public string? NationalIdentityNo { get; set; } = string.Empty;
    public string? TaxIdNo { get; set; } = string.Empty;
    public string? EmployeeId { get; set; } = string.Empty;
    public string? Title { get; set; } = string.Empty;
    public bool IsDeactivated { get; set; }
    public bool IsApprover { get; set; }
}
