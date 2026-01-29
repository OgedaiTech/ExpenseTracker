namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

public class ListPendingApprovalsResponse
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public decimal Amount { get; set; }
  public Guid CreatedByUserId { get; set; }
  public DateTime SubmittedAt { get; set; }
  public DateTime CreatedAt { get; set; }
}
