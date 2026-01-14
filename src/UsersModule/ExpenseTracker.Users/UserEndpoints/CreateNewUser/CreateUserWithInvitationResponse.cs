namespace ExpenseTracker.Users.UserEndpoints.CreateNewUser;

public record CreateUserWithInvitationResponse(
    string UserId,
    string Email,
    bool InvitationSent,
    string? Message = null);
