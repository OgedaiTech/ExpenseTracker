namespace ExpenseTracker.Expenses.Endpoints.ApproveExpense;

public interface IApproveExpenseRepository
{
  Task<Expense?> GetExpenseByIdAsync(Guid expenseId, Guid tenantId, CancellationToken cancellationToken);
  Task UpdateExpenseAsync(Expense expense, CancellationToken cancellationToken);
}
