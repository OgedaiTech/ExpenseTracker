namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public class SubmitExpenseResponse
{
  public Guid ExpenseId { get; set; }
  public ExpenseStatus Status { get; set; }
  public DateTime SubmittedAt { get; set; }
}
