using ExpenseTracker.Core;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public interface ICreateTenantService
{
  Task<ServiceResult<Guid>> CreateTenantAsync
    (CreateTenantRequest createReceiptRequest, CancellationToken ct);
}
