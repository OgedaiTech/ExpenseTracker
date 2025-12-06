using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Users.Contracts;

public record CreateUserCommand(Guid TenantId, string Email, string Password) : IRequest<ServiceResult>;
