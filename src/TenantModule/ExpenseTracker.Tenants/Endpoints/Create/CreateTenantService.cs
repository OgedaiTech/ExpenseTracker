using ExpenseTracker.Core;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantService(ICreateTenantRepository repository) : ICreateTenantService
{
  public async Task<ServiceResult> CreateTenantAsync(CreateTenantRequest createReceiptRequest, CancellationToken ct)
  {
    await repository.CreateTenantAsync(createReceiptRequest, ct);
    return new ServiceResult();
  }
}
