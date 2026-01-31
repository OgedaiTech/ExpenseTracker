using ExpenseTracker.Core;
using ExpenseTracker.Receipts.Contracts;
using ExpenseTracker.Receipts.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Receipts.Integrations;

public class GetReceiptCountQueryHandler(ReceiptDbContext dbContext)
  : IRequestHandler<GetReceiptCountQuery, ServiceResult<int>>
{
  public async Task<ServiceResult<int>> Handle(GetReceiptCountQuery request, CancellationToken cancellationToken)
  {
    var count = await dbContext.Receipts
      .Where(r => r.ExpenseId == request.ExpenseId)
      .CountAsync(cancellationToken);

    return new ServiceResult<int>(count);
  }
}
