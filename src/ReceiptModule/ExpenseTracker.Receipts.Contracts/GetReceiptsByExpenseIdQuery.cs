using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Receipts.Contracts;

public record GetReceiptsByExpenseIdQuery(Guid ExpenseId) : IRequest<ServiceResult<List<ReceiptSummaryDto>>>;

public record ReceiptSummaryDto(Guid Id, string ReceiptNo, DateTime Date, decimal Amount, string Vendor);
