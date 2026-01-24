namespace ExpenseTracker.Users.UserEndpoints.UpdateUser;

public record UpdateUserRequest(
    string? FirstName,
    string? LastName,
    string? NationalIdentityNo,
    string? TaxIdNo,
    string? EmployeeId,
    string? Title
);
