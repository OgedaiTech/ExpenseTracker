namespace ExpenseTracker.Tenants.Endpoints.Create;

public interface ICreateTenantRepository
{
  Task CreateTenantAsync(CreateTenantRequest createTenantRequest, CancellationToken ct);
}
