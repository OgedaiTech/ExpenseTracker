using System;

namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public record ExpenseDto(
  Guid Id,
  string Name,
  decimal Amount,
  DateTime CreatedAt,
  ExpenseStatus Status
);
