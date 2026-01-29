namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

public class ApproveExpenseResponse
{
  public Guid ExpenseId { get; set; }
  public ExpenseStatus Status { get; set; }
  public DateTime ApprovedAt { get; set; }
}
