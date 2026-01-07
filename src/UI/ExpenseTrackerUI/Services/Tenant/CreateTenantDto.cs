using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerUI.Services.Tenant;

public class CreateTenantDto
{
  [Required(ErrorMessage = "Tenant name is required")]
  [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
  public string Name { get; set; } = string.Empty;

  [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
  public string? Code { get; set; }

  [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
  public string? Description { get; set; }

  [StringLength(100, ErrorMessage = "Domain cannot exceed 100 characters")]
  public string? Domain { get; set; }

  [Required(ErrorMessage = "Email is required")]
  [EmailAddress(ErrorMessage = "Invalid email format")]
  public string Email { get; set; } = string.Empty;

  [Required(ErrorMessage = "Password is required")]
  [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
  [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
    ErrorMessage = "Password must contain uppercase, lowercase, number and special character")]
  public string Password { get; set; } = string.Empty;
}
