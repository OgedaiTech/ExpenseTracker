using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Users.Contracts;

public record GetUserEmailQuery(Guid UserId) : IRequest<ServiceResult<UserEmailDto>>;

public record UserEmailDto(string Email, string FirstName, string LastName);
