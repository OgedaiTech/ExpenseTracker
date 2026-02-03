namespace ExpenseTrackerUI.Services.Approval;

public record PendingApprovalDto(
  Guid Id,
  string Name,
  decimal Amount,
  Guid CreatedByUserId,
  DateTime SubmittedAt,
  DateTime CreatedAt
);
