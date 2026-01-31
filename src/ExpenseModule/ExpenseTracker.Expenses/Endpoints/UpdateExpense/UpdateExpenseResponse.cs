namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

public class UpdateExpenseResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public ExpenseStatus Status { get; set; }
  public DateTime UpdatedAt { get; set; }
}
