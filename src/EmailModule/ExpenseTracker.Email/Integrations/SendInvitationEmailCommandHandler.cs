using ExpenseTracker.Core;
using ExpenseTracker.Email.Contracts;
using MediatR;

namespace ExpenseTracker.Email.Integrations;

public class SendInvitationEmailCommandHandler(IEmailService emailService)
    : IRequestHandler<SendInvitationEmailCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        SendInvitationEmailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await emailService.SendInvitationEmailAsync(
                request.RecipientEmail,
                request.PasswordResetToken,
                cancellationToken);

            return new ServiceResult();
        }
        catch (Exception ex)
        {
            return new ServiceResult($"EMAIL_SEND_FAILED: {ex.Message}");
        }
    }
}
