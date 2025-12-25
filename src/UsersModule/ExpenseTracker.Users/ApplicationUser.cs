using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users;

public class ApplicationUser : IdentityUser
{
  public Guid TenantId { get; set; }
}
