namespace ExpenseTracker.Expenses.Endpoints.DeleteExpense;

public interface IDeleteExpenseRepository
{
  Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken);
  Task DeleteExpenseAsync(Expense expense, CancellationToken cancellationToken);
}
