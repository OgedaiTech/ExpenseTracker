using System;

namespace ExpenseTracker.Tenants;

public class Tenant
{
  public Guid Id { get; set; }

  public string Name { get; set; } = null!;
  public string? Code { get; set; }
  public string? Description { get; set; }
  public string? Domain { get; set; }

  public bool IsActive { get; set; } = true;

  public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public DateTime? UpdatedAt { get; set; }
}
