using ExpenseTracker.Expenses.Data;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Expenses.Endpoints.ListPendingApprovals;

public class ListPendingApprovalsRepository : IListPendingApprovalsRepository
{
  private readonly ExpenseDbContext _dbContext;

  public ListPendingApprovalsRepository(ExpenseDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public async Task<List<Expense>> GetPendingApprovalsAsync(Guid approverId, Guid tenantId, CancellationToken cancellationToken)
  {
    return await _dbContext.Expenses
      .Where(e => e.SubmittedToApproverId == approverId
        && e.TenantId == tenantId
        && e.Status == ExpenseStatus.Submitted)
      .OrderByDescending(e => e.SubmittedAt)
      .ToListAsync(cancellationToken);
  }
}
