namespace ExpenseTracker.Users.EmailService;

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
}
