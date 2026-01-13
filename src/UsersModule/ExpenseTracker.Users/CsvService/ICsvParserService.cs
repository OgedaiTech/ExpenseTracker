using ExpenseTracker.Users.UserEndpoints.BulkCreate;

namespace ExpenseTracker.Users.CsvService;

public interface ICsvParserService
{
  Task<List<UserCsvRecord>> ParseUserRecordsAsync(Stream csvStream, CancellationToken cancellationToken);
}
