using System;

namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantRequest
{
  public string Name { get; set; } = null!;
  public string? Code { get; set; }
  public string? Description { get; set; }
  public string? Domain { get; set; }
}
