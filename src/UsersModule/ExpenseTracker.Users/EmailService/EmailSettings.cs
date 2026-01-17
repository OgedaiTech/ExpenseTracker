namespace ExpenseTracker.Users.EmailService;

public class BulkUserCreationSettings
{
    public int MaxFileSizeBytes { get; set; } = 1048576;
    public int MaxUsersPerRequest { get; set; } = 500;
}
