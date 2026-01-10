namespace ExpenseTracker.Users.EmailService;

public class EmailSettings
{
    public string SmtpHost { get; set; } = string.Empty;
    public int SmtpPort { get; set; } = 587;
    public string SmtpUsername { get; set; } = string.Empty;
    public string SmtpPassword { get; set; } = string.Empty;
    public string FromAddress { get; set; } = string.Empty;
    public string FromName { get; set; } = string.Empty;
    public bool UseSsl { get; set; } = true;
}

public class BulkUserCreationSettings
{
    public int MaxFileSizeBytes { get; set; } = 1048576;
    public int MaxUsersPerRequest { get; set; } = 500;
    public string InvitationLinkBaseUrl { get; set; } = string.Empty;
}
