namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

public record BulkCreateUsersResponse(
    int TotalProcessed,
    int SuccessCount,
    int AlreadyExistedCount,
    int FailedCount,
    List<UserCreationResult> Results
);

public record UserCreationResult(
    string Email,
    UserCreationStatus Status,
    string? ErrorMessage = null
);

public enum UserCreationStatus
{
    Created,
    AlreadyExists,
    Failed,
    InvalidEmail
}
