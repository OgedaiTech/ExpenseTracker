namespace ExpenseTracker.Expenses.Endpoints.ListUsersExpenses;

public class BaseCollectionDto<T>
{
  public T? Items { get; set; }
  public int TotalCount { get; set; }
}
