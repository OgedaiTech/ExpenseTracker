using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Receipts.Contracts;

public record GetReceiptCountQuery(Guid ExpenseId) : IRequest<ServiceResult<int>>;
