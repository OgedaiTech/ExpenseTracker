using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Expenses.Contracts;

public record GetExpenseStatusQuery(Guid ExpenseId) : IRequest<ServiceResult<ExpenseStatusDto>>;

public record ExpenseStatusDto(int Status);
