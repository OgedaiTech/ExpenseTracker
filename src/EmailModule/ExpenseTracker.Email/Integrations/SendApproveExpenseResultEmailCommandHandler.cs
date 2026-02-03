using System;
using ExpenseTracker.Core;
using ExpenseTracker.Email.Contracts;
using MediatR;

namespace ExpenseTracker.Email.Integrations;

public class SendApproveExpenseResultEmailCommandHandler(IEmailService emailService) : IRequestHandler<SendApproveExpenseResultEmailCommand, ServiceResult>
{
  public async Task<ServiceResult> Handle(SendApproveExpenseResultEmailCommand request, CancellationToken cancellationToken)
  {
    var subject = "Your expense has been approved";
    var body = $"Hello,\n\nYour expense titled '{request.ExpenseTitle}' has been approved by {request.ApprovedByEmail}.\n\nBest regards,\nExpense Tracker Team";

    var emailResult = await emailService.SendApproveExpenseResultEmailAsync(
        to: request.SubmittedByEmail,
        subject: subject,
        body: body,
        cancellationToken: cancellationToken);

    if (!emailResult.Success)
    {
      return new ServiceResult("Failed to send approval email");
    }

    return new ServiceResult();
  }
}
