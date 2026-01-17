namespace ExpenseTracker.Email;

public interface IEmailService
{
    Task SendInvitationEmailAsync(
        string recipientEmail,
        string passwordResetToken,
        CancellationToken cancellationToken);
}
