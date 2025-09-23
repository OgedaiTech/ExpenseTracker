namespace ExpenseTracker.Expenses.Endpoints.Create;

public class CreateExpenseService(ICreateExpenseRepository createExpenseRepository) : ICreateExpenseService
{
  public async Task<ServiceResult> CreateExpenseAsync(string name, CancellationToken cancellationToken)
  {
    if (string.IsNullOrWhiteSpace(name))
    {
      return new ServiceResult(message: "Expense name cannot be empty.");
    }
    else if (name.Length > 128)
    {
      return new ServiceResult(message: "Expense name cannot exceed 128 characters.");
    }
    await createExpenseRepository.CreateExpenseAsync(name, cancellationToken);
    return new ServiceResult();
  }
}
