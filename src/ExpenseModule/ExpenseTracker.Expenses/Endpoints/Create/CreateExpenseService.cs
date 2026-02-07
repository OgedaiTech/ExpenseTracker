using ExpenseTracker.Core;

namespace ExpenseTracker.Expenses.Endpoints.Create;

public class CreateExpenseService(ICreateExpenseRepository createExpenseRepository) : ICreateExpenseService
{
  public async Task<ServiceResult> CreateExpenseAsync(string name, string userId, string tenantId, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return new ServiceResult(message: CreateExpenseConstants.ExpenseNameCannotBeEmpty);
    }
    else if (name.Length > 128)
    {
      return new ServiceResult(message: CreateExpenseConstants.ExpenseNameCannotExceed128Characters);
    }
    await createExpenseRepository.CreateExpenseAsync(name, userId, tenantId, cancellationToken);
    return new ServiceResult();
  }
}
