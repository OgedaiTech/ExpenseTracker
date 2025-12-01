namespace ExpenseTracker.Tenants.Endpoints.Create;

public interface ICreateTenantRepository
{
  Task<Guid> CreateTenantAsync(CreateTenantRequest createTenantRequest, CancellationToken ct);
  Task<bool> TenantExistsAsync(string code);
}
