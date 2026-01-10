namespace ExpenseTracker.Users.EmailService;

public interface IEmailService
{
    Task SendInvitationEmailAsync(
        string recipientEmail,
        string passwordResetToken,
        CancellationToken cancellationToken);
}
