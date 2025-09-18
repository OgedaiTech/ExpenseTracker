using System;

namespace ExpenseTracker.Expenses.Endpoints.Create;

public class CreateExpenseService(ICreateExpenseRepository createExpenseRepository) : ICreateExpenseService
{
  public Task CreateExpenseAsync()
  {
    return createExpenseRepository.CreateExpenseAsync();
  }
}
