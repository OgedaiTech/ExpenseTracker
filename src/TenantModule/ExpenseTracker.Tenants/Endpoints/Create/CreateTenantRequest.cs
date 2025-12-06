namespace ExpenseTracker.Tenants.Endpoints.Create;

public class CreateTenantRequest
{
  public string Name { get; set; } = null!;
  public string? Code { get; set; }
  public string? Description { get; set; }
  public string? Domain { get; set; }
  public required string Email { get; set; }
  public required string Password { get; set; }
}
