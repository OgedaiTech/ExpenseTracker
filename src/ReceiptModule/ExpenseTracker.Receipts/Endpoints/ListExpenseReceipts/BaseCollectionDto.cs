namespace ExpenseTracker.Receipts.Endpoints.ListExpenseReceipts;

public class BaseCollectionDto<T>
{
    public T? Items { get; set; }
    public int TotalCount { get; set; }
}
