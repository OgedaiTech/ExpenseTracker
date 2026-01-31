using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Users.Contracts;

public record IsUserApproverQuery(Guid UserId, Guid TenantId) : IRequest<ServiceResult<bool>>;
