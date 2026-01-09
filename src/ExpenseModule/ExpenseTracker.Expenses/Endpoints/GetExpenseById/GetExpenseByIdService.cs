using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.GetExpenseById;

public class GetExpenseByIdService(IGetExpenseByIdRepository repository) : IGetExpenseByIdService
{
    public async Task<ServiceResult<Expense>> GetExpenseByIdAsync(string expenseId, string userId, string tenantId, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(expenseId) || !Guid.TryParse(expenseId, out var parsedExpenseId) || parsedExpenseId == Guid.Empty)
        {
            return new ServiceResult<Expense>("Invalid expense ID.");
        }

        var expense = await repository.GetExpenseByIdAsync(expenseId, userId, tenantId, ct);

        if (expense == null)
        {
            return new ServiceResult<Expense>("Expense not found.");
        }

        return new ServiceResult<Expense>(expense);
    }
}
