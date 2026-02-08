using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantService(ICreateTenantRepository repository,
  IMediator mediator) : ICreateTenantService
{
  public async Task<ServiceResult> CreateTenantAsync(CreateTenantRequest request, CancellationToken ct)
  {
    var validationResult = await ValidateTenantAsync(request.Code!);
    if (!validationResult.Success)
    {
      return new ServiceResult(validationResult.Message!);
    }

    var tenantId = await repository.CreateTenantAsync(request, ct);

    var command = new CreateTenantAdminUserCommand(tenantId, request.Email, request.Password);
    var createTenantAdminResult = await mediator.Send(command, ct);
    if (!createTenantAdminResult.Success)
    {
      await repository.DeleteTenantAsync(tenantId);
      if (createTenantAdminResult.Message == CreateTenantConstants.TenantAdminUserEmailAlreadyExists)
      {
        return new ServiceResult(CreateTenantConstants.TenantAdminUserEmailAlreadyExists);
      }
      return new ServiceResult(CreateTenantConstants.CantCreateTenantAdminUser);
    }
    return new ServiceResult();
  }

  private async Task<ServiceResult> ValidateTenantAsync(string code)
  {
    if (string.IsNullOrEmpty(code))
    {
      return new ServiceResult(CreateTenantConstants.TenantCodeIsRequired);
    }

    var result = await repository.TenantExistsAsync(code);
    if (result)
    {
      return new ServiceResult("TENANT_CODE_ALREADY_EXISTS");
    }
    return new ServiceResult();
  }
}
