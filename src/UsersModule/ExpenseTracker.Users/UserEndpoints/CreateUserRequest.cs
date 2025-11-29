namespace ExpenseTracker.Users.UserEndpoints;

public record CreateUserRequest(string Email, string Password, Guid? TenantId);
