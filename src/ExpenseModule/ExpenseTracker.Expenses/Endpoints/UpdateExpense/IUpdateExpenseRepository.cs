namespace ExpenseTracker.Expenses.Endpoints.UpdateExpense;

public interface IUpdateExpenseRepository
{
  Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken);
  Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken);
}
