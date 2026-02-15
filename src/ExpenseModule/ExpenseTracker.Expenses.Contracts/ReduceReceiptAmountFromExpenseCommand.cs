using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Expenses.Contracts;

public record ReduceReceiptAmountFromExpenseCommand(Guid ExpenseId, decimal Amount)
  : IRequest<ServiceResult>;
