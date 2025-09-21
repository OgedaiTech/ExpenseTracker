namespace ExpenseTracker.Expenses;

public class Expense
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public DateTime? DeletedAt { get; set; }
}
