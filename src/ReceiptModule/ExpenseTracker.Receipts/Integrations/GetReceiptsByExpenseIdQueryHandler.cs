using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Contracts;
using ExpenseTracker.Receipts.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Integrations;

public class GetReceiptsByExpenseIdQueryHandler(ReceiptDbContext dbContext)
  : IRequestHandler<GetReceiptsByExpenseIdQuery, ServiceResult<List<ReceiptSummaryDto>>>
{
  public async Task<ServiceResult<List<ReceiptSummaryDto>>> Handle(
    GetReceiptsByExpenseIdQuery request,
    CancellationToken cancellationToken)
  {
    var receipts = await dbContext.Receipts
      .Where(r => r.ExpenseId == request.ExpenseId)
      .Select(r => new ReceiptSummaryDto(r.Id, r.ReceiptNo, r.Date, r.Amount, r.Vendor))
      .ToListAsync(cancellationToken);

    return new ServiceResult<List<ReceiptSummaryDto>>(receipts);
  }
}
