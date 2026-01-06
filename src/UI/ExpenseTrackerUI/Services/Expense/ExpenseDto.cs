namespace ExpenseTrackerUI.Services.Expense;

public record ExpenseDto(
  Guid Id,
  string Name,
  decimal Amount,
  DateTime CreatedAt
);
