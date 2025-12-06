using ExpenseTracker.Core;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public interface ICreateTenantService
{
  Task<ServiceResult> CreateTenantAsync
    (CreateTenantRequest request, CancellationToken ct);
}
