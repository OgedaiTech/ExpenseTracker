namespace ExpenseTracker.Email;

public static class EmailTemplates
{
  public static (string Subject, string HtmlBody, string TextBody) GetInvitationEmail(string invitationLink)
  {
    var subject = "You've been invited to ExpenseTracker";

    var htmlBody = $"""
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h1>Welcome to ExpenseTracker</h1>
                <p>You have been invited to join ExpenseTracker.
                   Please click the button below to set your password and activate your account.</p>
                <p style="margin: 30px 0;">
                    <a href="{invitationLink}"
                       style="background-color: #007bff; color: white; padding: 12px 24px;
                              text-decoration: none; border-radius: 4px;">
                        Set Your Password
                    </a>
                </p>
                <p>This link will expire in 24 hours.</p>
                <p>If you did not expect this invitation, you can safely ignore this email.</p>
            </body>
            </html>
            """;

    var textBody = $"""
            Welcome to ExpenseTracker

            You have been invited to join ExpenseTracker.
            Please visit the following link to set your password and activate your account:

            {invitationLink}

            This link will expire in 24 hours.

            If you did not expect this invitation, you can safely ignore this email.
            """;

    return (subject, htmlBody, textBody);
  }

  public static (string Subject, string HtmlBody, string TextBody) GetPasswordResetEmail(string resetLink)
  {
    var subject = "Password Reset Request - ExpenseTracker";

    var htmlBody = $"""
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h1>Password Reset Request</h1>
                <p>You have requested to reset your password for your ExpenseTracker account.
                   Please click the button below to set a new password.</p>
                <p style="margin: 30px 0;">
                    <a href="{resetLink}"
                       style="background-color: #007bff; color: white; padding: 12px 24px;
                              text-decoration: none; border-radius: 4px;">
                        Reset Your Password
                    </a>
                </p>
                <p>This link will expire in 24 hours.</p>
                <p><strong>If you did not request a password reset, please ignore this email.</strong>
                   Your password will remain unchanged.</p>
            </body>
            </html>
            """;

    var textBody = $"""
            Password Reset Request

            You have requested to reset your password for your ExpenseTracker account.
            Please visit the following link to set a new password:

            {resetLink}

            This link will expire in 24 hours.

            If you did not request a password reset, please ignore this email.
            Your password will remain unchanged.
            """;

    return (subject, htmlBody, textBody);
  }

  public static (string Subject, string HtmlBody, string TextBody) GetSubmitExpenseToApproverEmail(string expenseName, string approverEmail, string submitterName)
  {
    var subject = "New Expense Submitted for Your Approval";

    var htmlBody = $"""
            <html>
            <body style="font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto;">
                <h1>Expense Submitted for Approval</h1>
                <p>Dear {approverEmail},</p>
                <p>{submitterName} has submitted a new expense titled "<strong>{expenseName}</strong>" for your approval.</p>
                <p>Please log in to the ExpenseTracker system to review and approve the expense at your earliest convenience.</p>
                <p>Thank you for your attention to this matter.</p>
            </body>
            </html>
            """;

    var textBody = $"""
            Expense Submitted for Approval

            Dear {approverEmail},

            {submitterName} has submitted a new expense titled "{expenseName}" for your approval.

            Please log in to the ExpenseTracker system to review and approve the expense at your earliest convenience.

            Thank you for your attention to this matter.
            """;

    return (subject, htmlBody, textBody);
  }
}
