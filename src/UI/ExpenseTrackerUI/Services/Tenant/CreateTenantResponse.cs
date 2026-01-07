namespace ExpenseTrackerUI.Services.Tenant;

public class CreateTenantResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public string Code { get; set; } = string.Empty;
}
