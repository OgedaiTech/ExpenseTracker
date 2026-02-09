using System;

namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

internal static class UpdateExpenseConstants
{
  public const string ExpenseNameEmptyErrorMessage = "Expense name cannot be empty";
  public const string ExpenseNameTooLongErrorMessage = $"Expense name cannot exceed 128 characters";
  public const string ExpenseNotFoundErrorMessage = "Expense not found";
  public const string ExpenseOwnershipErrorMessage = "You can only update your own expenses";
  public const string ExpenseStatusImmutableErrorMessage = "Cannot update expense. Only draft or rejected expenses can be modified.";
}
