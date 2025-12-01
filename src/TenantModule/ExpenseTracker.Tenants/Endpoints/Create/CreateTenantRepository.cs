using ExpenseTracker.Tenants.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantRepository(TenantDbContext tenantDbContext) : ICreateTenantRepository
{
  public async Task<Guid> CreateTenantAsync(CreateTenantRequest createTenantRequest, CancellationToken ct)
  {
    tenantDbContext.Tenants.Add(new Tenant
    {
      Name = createTenantRequest.Name,
      Code = createTenantRequest.Code,
      Description = createTenantRequest.Description,
      Domain = createTenantRequest.Domain
    });
    await tenantDbContext.SaveChangesAsync(ct);
    var tenant = await tenantDbContext.Tenants.FirstOrDefaultAsync(t => t.Code == createTenantRequest.Code, ct);
    return tenant!.Id;
  }

  public Task<bool> TenantExistsAsync(string code)
  {
    return tenantDbContext.Tenants.AnyAsync(t => t.Code == code);
  }
}
