using ExpenseTracker.Core;

namespace ExpenseTracker.Email;

public interface IEmailService
{
  Task SendInvitationEmailAsync(
      string recipientEmail,
      string passwordResetToken,
      CancellationToken cancellationToken);

  Task SendPasswordResetEmailAsync(
      string recipientEmail,
      string passwordResetToken,
      CancellationToken cancellationToken);

  Task SendSubmitExpenseToApproverEmailAsync(
      string expenseName,
      string approverEmail,
      string submitterName,
      CancellationToken cancellationToken);

  Task<ServiceResult> SendApproveExpenseResultEmailAsync(
      string to,
      string subject,
      string body,
      CancellationToken cancellationToken);
}
