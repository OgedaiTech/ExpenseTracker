namespace ExpenseTracker.Tenants.Endpoints.Create;

public interface ICreateTenantRepository
{
  Task<Guid> CreateTenantAsync(CreateTenantRequest createTenantRequest, CancellationToken ct);
  Task DeleteTenantAsync(Guid tenantId);
  Task<bool> TenantExistsAsync(string code);
}
