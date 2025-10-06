using ExpenseTracker.Core;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public interface ICreateTenantService
{
  Task<ServiceResult> CreateTenantAsync
    (CreateTenantRequest createReceiptRequest, CancellationToken ct);
}
