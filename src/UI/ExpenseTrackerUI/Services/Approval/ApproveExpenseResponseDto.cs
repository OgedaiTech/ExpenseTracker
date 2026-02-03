using ExpenseTrackerUI.Services.Expense;

namespace ExpenseTrackerUI.Services.Approval;

public class ApproveExpenseResponseDto
{
  public Guid ExpenseId { get; set; }
  public ExpenseStatus Status { get; set; }
  public DateTime ApprovedAt { get; set; }
}
