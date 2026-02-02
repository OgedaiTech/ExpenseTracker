namespace ExpenseTracker.Expenses.Endpoints.SubmitExpense;

public static class SubmitExpenseConstants
{
  public const string FailedToRetrieveSubmitterEmail = "Failed to retrieve submitter email information";
  public const string FailedToRetrieveApproverEmail = "Failed to retrieve approver email information";
  public const string SelectedApproverIsNotInYourOrganization = "The selected user is not an approver in your organization";
  public const string CannotSubmitExpenseWithoutReceipts = "Cannot submit expense without at least one receipt";
  public const string OnlyDraftOrRejectedExpensesCanBeSubmitted = "Only draft or rejected expenses can be submitted for approval";
  public const string YouCanOnlySubmitYourOwnExpenses = "You can only submit your own expenses";
  public const string ExpenseNotFound = "Expense not found";
  public const string YouCanNotSubmitExpensesToYourselfForApproval = "You cannot submit expenses to yourself for approval";
}
