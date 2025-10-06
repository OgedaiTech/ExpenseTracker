using ExpenseTracker.Tenants.Data;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantRepository(TenantDbContext tenantDbContext) : ICreateTenantRepository
{
  public Task CreateTenantAsync(CreateTenantRequest createTenantRequest, CancellationToken ct)
  {
    tenantDbContext.Tenants.Add(new Tenant
    {
      Name = createTenantRequest.Name,
      Code = createTenantRequest.Code,
      Description = createTenantRequest.Description,
      Domain = createTenantRequest.Domain
    });
    return tenantDbContext.SaveChangesAsync(ct);
  }
}
