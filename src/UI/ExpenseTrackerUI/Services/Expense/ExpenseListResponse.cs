namespace ExpenseTrackerUI.Services.Expense;

public class ExpenseListResponse
{
    public List<ExpenseDto>? Items { get; set; }
    public int TotalCount { get; set; }
}
