namespace ExpenseTracker.Users.UserEndpoints.SetPassword;

public record SetPasswordRequest(
    string Email,
    string Token,
    string NewPassword,
    string ConfirmPassword
);
