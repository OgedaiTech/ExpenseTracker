using System.Web;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ExpenseTracker.Email;

public partial class SmtpEmailService(
    IOptions<EmailSettings> emailSettings,
    IOptions<InvitationEmailSettings> invitationEmailSettings,
    ILogger<SmtpEmailService> logger) : IEmailService
{
  private readonly EmailSettings _emailSettings = emailSettings.Value;
  private readonly InvitationEmailSettings _invitationEmailSettings = invitationEmailSettings.Value;

  public async Task SendInvitationEmailAsync(
      string recipientEmail,
      string passwordResetToken,
      CancellationToken cancellationToken)
  {
    var invitationLink = BuildInvitationLink(recipientEmail, passwordResetToken);
    var (subject, htmlBody, textBody) = EmailTemplates.GetInvitationEmail(invitationLink);

    await SendEmailAsync(recipientEmail, subject, htmlBody, textBody, cancellationToken);
    LogInvitationEmailSent(logger, recipientEmail);
  }

  public async Task SendPasswordResetEmailAsync(
      string recipientEmail,
      string passwordResetToken,
      CancellationToken cancellationToken)
  {
    var resetLink = BuildInvitationLink(recipientEmail, passwordResetToken);
    var (subject, htmlBody, textBody) = EmailTemplates.GetPasswordResetEmail(resetLink);

    await SendEmailAsync(recipientEmail, subject, htmlBody, textBody, cancellationToken);
    LogPasswordResetEmailSent(logger, recipientEmail);
  }

  private async Task SendEmailAsync(
      string recipientEmail,
      string subject,
      string htmlBody,
      string textBody,
      CancellationToken cancellationToken)
  {
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
    message.To.Add(new MailboxAddress(recipientEmail, recipientEmail));
    message.Subject = subject;

    var bodyBuilder = new BodyBuilder
    {
      HtmlBody = htmlBody,
      TextBody = textBody
    };
    message.Body = bodyBuilder.ToMessageBody();

    using var client = new SmtpClient();

    try
    {
      // Use STARTTLS for port 587 (Gmail), direct SSL for port 465
      var secureSocketOptions = _emailSettings.SmtpPort == 465
          ? SecureSocketOptions.SslOnConnect
          : SecureSocketOptions.StartTls;

      await client.ConnectAsync(
          _emailSettings.SmtpHost,
          _emailSettings.SmtpPort,
          secureSocketOptions,
          cancellationToken);

      if (!string.IsNullOrEmpty(_emailSettings.SmtpUsername))
      {
        await client.AuthenticateAsync(
            _emailSettings.SmtpUsername,
            _emailSettings.SmtpPassword,
            cancellationToken);
      }

      await client.SendAsync(message, cancellationToken);
    }
    finally
    {
      await client.DisconnectAsync(true, cancellationToken);
    }
  }

  public Task SendSubmitExpenseToApproverEmailAsync(string expenseName, string approverEmail, string submitterName, CancellationToken cancellationToken)
  {
    var (subject, htmlBody, textBody) = EmailTemplates.GetSubmitExpenseToApproverEmail(expenseName, approverEmail, submitterName);

    return SendEmailAsync(approverEmail, subject, htmlBody, textBody, cancellationToken);
  }

  private string BuildInvitationLink(string email, string token)
  {
    var encodedEmail = HttpUtility.UrlEncode(email);
    var encodedToken = HttpUtility.UrlEncode(token);
    return $"{_invitationEmailSettings.InvitationLinkBaseUrl}?email={encodedEmail}&token={encodedToken}";
  }

  [LoggerMessage(
      EventId = 200,
      Level = LogLevel.Information,
      Message = "Invitation email sent to {Email}")]
  private static partial void LogInvitationEmailSent(ILogger logger, string email);

  [LoggerMessage(
      EventId = 201,
      Level = LogLevel.Information,
      Message = "Password reset email sent to {Email}")]
  private static partial void LogPasswordResetEmailSent(ILogger logger, string email);
}
