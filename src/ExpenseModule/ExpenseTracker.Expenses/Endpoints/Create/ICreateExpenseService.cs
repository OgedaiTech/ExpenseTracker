using System;

namespace ExpenseTracker.Expenses.Endpoints.Create;

public interface ICreateExpenseService
{
  Task CreateExpenseAsync();
}
