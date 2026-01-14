using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users;

public class ApplicationUser : IdentityUser
{
  public Guid TenantId { get; set; }

  // Employee Details
  public string? FirstName { get; set; }
  public string? LastName { get; set; }
  public string? NationalIdentityNo { get; set; }
  public string? TaxIdNo { get; set; }
  public string? EmployeeId { get; set; }
  public string? Title { get; set; }

  // User Status
  public bool IsDeactivated { get; set; } = false; // Default to active
}
