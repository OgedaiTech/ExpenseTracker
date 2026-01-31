namespace ExpenseTracker.Expenses.Endpoints.RejectExpense;

public class RejectExpenseRequest
{
  public string RejectionReason { get; set; } = string.Empty;
}
