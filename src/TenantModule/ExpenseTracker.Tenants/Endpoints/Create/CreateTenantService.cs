using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantService(ICreateTenantRepository repository,
  IMediator mediator) : ICreateTenantService
{
  public async Task<ServiceResult> CreateTenantAsync(CreateTenantRequest createTenantRequest, CancellationToken ct)
  {
    var validationResult = await ValidateTenantAsync(createTenantRequest.Code!);
    if (!validationResult.Success)
    {
      return new ServiceResult(validationResult.Message!);
    }
    var tenantId = await repository.CreateTenantAsync(createTenantRequest, ct);

    // TODO: Remove hardcoded email
    var command = new CreateTenantAdminUserCommand(tenantId, "admin@ttt.com");
    var createTenantAdminResult = await mediator.Send(command, ct);
    if (!createTenantAdminResult.Success)
    {
      return new ServiceResult("CANT_CREATE_TENANT_ADMIN_USER");
    }
    return new ServiceResult();
  }

  private async Task<ServiceResult> ValidateTenantAsync(string code)
  {
    if (string.IsNullOrEmpty(code))
    {
      return new ServiceResult("TENANT_CODE_IS_REQUIRED");
    }

    var result = await repository.TenantExistsAsync(code);
    if (result)
    {
      return new ServiceResult("TENANT_CODE_ALREADY_EXISTS");
    }
    return new ServiceResult();
  }
}
