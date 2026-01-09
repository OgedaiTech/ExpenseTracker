namespace ExpenseTracker.Expenses.Endpoints.GetExpenseById;

public interface IGetExpenseByIdRepository
{
    Task<Expense?> GetExpenseByIdAsync(string expenseId, string userId, string tenantId, CancellationToken ct);
}
