using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Email.Contracts;

public record SendInvitationEmailCommand(
    string RecipientEmail,
    string PasswordResetToken) : IRequest<ServiceResult>;
