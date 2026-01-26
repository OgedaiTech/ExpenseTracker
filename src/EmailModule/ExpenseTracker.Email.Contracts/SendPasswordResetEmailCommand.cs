using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Email.Contracts;

public record SendPasswordResetEmailCommand(
    string RecipientEmail,
    string PasswordResetToken) : IRequest<ServiceResult>;
