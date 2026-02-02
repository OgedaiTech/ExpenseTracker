using ExpenseTracker.Core;
using ExpenseTracker.Email.Contracts;
using MediatR;

namespace ExpenseTracker.Email.Integrations;

public class SendSubmitExpenseToApproverEmailCommandHandler(IEmailService emailService)
    : IRequestHandler<SendSubmitExpenseToApproverEmailCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(
      SendSubmitExpenseToApproverEmailCommand request,
      CancellationToken cancellationToken)
  {
    try
    {
      await emailService.SendSubmitExpenseToApproverEmailAsync(
          request.ExpenseName,
          request.ApproverEmail,
          request.SubmitterName,
          cancellationToken);

      return new ServiceResult();
    }
    catch (Exception ex)
    {
      return new ServiceResult($"EMAIL_SEND_FAILED: {ex.Message}");
    }
  }
}
