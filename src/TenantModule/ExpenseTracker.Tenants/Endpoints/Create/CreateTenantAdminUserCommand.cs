using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public record CreateTenantAdminUserCommand(Guid TenantId, string AdminEmail) : IRequest<ServiceResult>;
