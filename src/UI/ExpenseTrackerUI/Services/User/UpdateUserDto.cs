namespace ExpenseTrackerUI.Services.User;

public class UpdateUserDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? NationalIdentityNo { get; set; }
    public string? TaxIdNo { get; set; }
    public string? EmployeeId { get; set; }
    public string? Title { get; set; }
}

public class UpdateUserResponse
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
