using ExpenseTracker.Core;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantService(ICreateTenantRepository repository) : ICreateTenantService
{
  public async Task<ServiceResult<Guid>> CreateTenantAsync(CreateTenantRequest createReceiptRequest, CancellationToken ct)
  {
    var tenantId = await repository.CreateTenantAsync(createReceiptRequest, ct);
    return new ServiceResult<Guid>(tenantId);
  }
}
