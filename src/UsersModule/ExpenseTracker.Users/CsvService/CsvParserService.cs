using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using ExpenseTracker.Users.UserEndpoints.BulkCreate;

namespace ExpenseTracker.Users.CsvService;

public class CsvParserService : ICsvParserService
{
  public async Task<List<UserCsvRecord>> ParseUserRecordsAsync(Stream csvStream, CancellationToken cancellationToken)
  {
    var records = new List<UserCsvRecord>();

    using var reader = new StreamReader(csvStream);
    var config = new CsvConfiguration(CultureInfo.InvariantCulture)
    {
      HasHeaderRecord = true,
      MissingFieldFound = null,
      BadDataFound = null,
      TrimOptions = TrimOptions.Trim
    };

    using var csv = new CsvReader(reader, config);

    csv.Context.RegisterClassMap<UserCsvRecordMap>();

    await foreach (var record in csv.GetRecordsAsync<UserCsvRecord>(cancellationToken))
    {
      records.Add(record);
    }

    return records;
  }
}
