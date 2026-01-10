namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

public interface IBulkCreateUsersService
{
    Task<BulkCreateUsersResult> CreateUsersFromCsvAsync(
        Stream csvStream,
        Guid tenantId,
        CancellationToken cancellationToken);
}

public record BulkCreateUsersResult(
    bool Success,
    string? ErrorCode = null,
    string? ErrorMessage = null,
    BulkCreateUsersResponse? Response = null
);
