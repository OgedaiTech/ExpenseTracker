using ExpenseTracker.Core;
using ExpenseTracker.Email.Contracts;
using MediatR;

namespace ExpenseTracker.Email.Integrations;

public class SendPasswordResetEmailCommandHandler(IEmailService emailService)
    : IRequestHandler<SendPasswordResetEmailCommand, ServiceResult>
{
    public async Task<ServiceResult> Handle(
        SendPasswordResetEmailCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            await emailService.SendPasswordResetEmailAsync(
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
