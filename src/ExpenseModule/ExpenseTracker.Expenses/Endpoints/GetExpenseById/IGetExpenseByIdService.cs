using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.GetExpenseById;

public interface IGetExpenseByIdService
{
    Task<ServiceResult<Expense>> GetExpenseByIdAsync(string expenseId, string userId, string tenantId, CancellationToken ct);
}
