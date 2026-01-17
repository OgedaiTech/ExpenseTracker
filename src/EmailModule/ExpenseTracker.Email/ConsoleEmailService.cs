using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExpenseTracker.Email;

public partial class ConsoleEmailService(
    IOptions<InvitationEmailSettings> invitationEmailSettings,
    ILogger<ConsoleEmailService> logger) : IEmailService
{
    private readonly InvitationEmailSettings _invitationEmailSettings = invitationEmailSettings.Value;

    public Task SendInvitationEmailAsync(
        string recipientEmail,
        string passwordResetToken,
        CancellationToken cancellationToken)
    {
        var invitationLink = BuildInvitationLink(recipientEmail, passwordResetToken);
        var (subject, htmlBody, textBody) = EmailTemplates.GetInvitationEmail(invitationLink);

        LogEmailDetails(logger, recipientEmail, subject);
        LogInvitationLink(logger, invitationLink);
        LogPasswordResetToken(logger, passwordResetToken);

        Console.WriteLine();
        Console.WriteLine("========================================");
        Console.WriteLine($"📧 EMAIL SENT (Development Mode)");
        Console.WriteLine("========================================");
        Console.WriteLine($"To: {recipientEmail}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine();
        Console.WriteLine("Invitation Link:");
        Console.WriteLine(invitationLink);
        Console.WriteLine();
        Console.WriteLine("Password Reset Token (for manual testing):");
        Console.WriteLine(passwordResetToken);
        Console.WriteLine("========================================");
        Console.WriteLine();

        return Task.CompletedTask;
    }

    private string BuildInvitationLink(string email, string token)
    {
        var encodedEmail = System.Web.HttpUtility.UrlEncode(email);
        var encodedToken = System.Web.HttpUtility.UrlEncode(token);
        return $"{_invitationEmailSettings.InvitationLinkBaseUrl}?email={encodedEmail}&token={encodedToken}";
    }

    [LoggerMessage(
        EventId = 201,
        Level = LogLevel.Information,
        Message = "[DEV] Email would be sent to {Email} with subject: {Subject}")]
    private static partial void LogEmailDetails(ILogger logger, string email, string subject);

    [LoggerMessage(
        EventId = 202,
        Level = LogLevel.Information,
        Message = "[DEV] Invitation link: {InvitationLink}")]
    private static partial void LogInvitationLink(ILogger logger, string invitationLink);

    [LoggerMessage(
        EventId = 203,
        Level = LogLevel.Information,
        Message = "[DEV] Password reset token: {Token}")]
    private static partial void LogPasswordResetToken(ILogger logger, string token);
}
