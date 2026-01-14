namespace ExpenseTracker.Users.UserEndpoints.CreateNewUser;

public record CreateUserWithInvitationRequest(
    string Email,
    string? FirstName = null,
    string? LastName = null,
    string? NationalIdentityNo = null,
    string? TaxIdNo = null,
    string? EmployeeId = null,
    string? Title = null);
