namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

public record UserCsvRecord(
  string Email,
  string? FirstName,
  string? LastName,
  string? NationalIdentityNo,
  string? TaxIdNo,
  string? EmployeeId,
  string? Title);
