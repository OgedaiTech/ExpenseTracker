using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Receipts.UseCases;

public record GetReceiptCountQuery(Guid ExpenseId) : IRequest<ServiceResult<int>>;
