namespace ExpenseTracker.Expenses;

public class Expense
{
  public Guid Id { get; set; }
  public string Name { get; set; } = string.Empty;
  public Guid CreatedByUserId { get; set; }
  public Guid TenantId { get; set; }
  public decimal Amount { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  public DateTime? DeletedAt { get; set; }

  // Approval workflow fields
  public ExpenseStatus Status { get; set; } = ExpenseStatus.Draft;
  public Guid? SubmittedToApproverId { get; set; }
  public DateTime? SubmittedAt { get; set; }
  public Guid? ApprovedByUserId { get; set; }
  public DateTime? ApprovedAt { get; set; }
  public Guid? RejectedByUserId { get; set; }
  public DateTime? RejectedAt { get; set; }
  public string? RejectionReason { get; set; }
}
