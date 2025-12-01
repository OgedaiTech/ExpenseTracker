using ExpenseTracker.Core;
using ExpenseTracker.Users.Contracts;
using MediatR;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantAdminUserCommandHandler(IMediator mediator) : IRequestHandler<CreateTenantAdminUserCommand, ServiceResult>
{
  public Task<ServiceResult> Handle(CreateTenantAdminUserCommand request, CancellationToken cancellationToken)
  {
    var command = new CreateUserCommand(request.TenantId, request.AdminEmail);
    return mediator.Send(command, cancellationToken);
  }
}
