using ExpenseTracker.Core;
using MediatR;

namespace ExpenseTracker.Users.Contracts;

public record CreateUserCommand(
  Guid TenantId,
  string Email,
  string Password,
  string? FirstName = null,
  string? LastName = null,
  string? NationalIdentityNo = null,
  string? TaxIdNo = null,
  string? EmployeeId = null,
  string? Title = null) : IRequest<ServiceResult>;
