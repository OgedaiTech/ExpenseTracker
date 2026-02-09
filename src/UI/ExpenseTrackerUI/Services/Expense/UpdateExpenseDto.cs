using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerUI.Services.Expense;

public class UpdateExpenseDto
{
  [Required(ErrorMessage = "Expense name is required")]
  [StringLength(128, ErrorMessage = "Expense name cannot exceed 128 characters")]
  public string Name { get; set; } = string.Empty;
}

public class UpdateExpenseResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public ExpenseStatus Status { get; set; }
  public DateTime UpdatedAt { get; set; }
}
