namespace ExpenseTracker.Expenses.Endpoints.RejectExpense;

public class RejectExpenseResponse
{
  public Guid ExpenseId { get; set; }
  public ExpenseStatus Status { get; set; }
  public DateTime RejectedAt { get; set; }
  public string RejectionReason { get; set; } = string.Empty;
}
