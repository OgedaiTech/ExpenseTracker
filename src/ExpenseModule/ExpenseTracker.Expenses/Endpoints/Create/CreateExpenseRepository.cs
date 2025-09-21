using ExpenseTracker.Expenses.Data;

namespace ExpenseTracker.Expenses.Endpoints.Create;

public class CreateExpenseRepository : ICreateExpenseRepository
{
  private readonly ExpenseDbContext _dbContext;

  public CreateExpenseRepository(ExpenseDbContext dbContext)
  {
    _dbContext = dbContext;
  }
  public Task CreateExpenseAsync(string name, CancellationToken cancellationToken)
  {
    _dbContext.Expenses.Add(new Expense
    {
      Name = name,
      CreatedAt = DateTime.UtcNow
    });
    return _dbContext.SaveChangesAsync();
  }
}
