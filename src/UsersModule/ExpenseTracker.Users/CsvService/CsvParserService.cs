using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace ExpenseTracker.Users.CsvService;

public class CsvParserService : ICsvParserService
{
    public async Task<List<string>> ParseEmailsAsync(Stream csvStream, CancellationToken cancellationToken)
    {
        var emails = new List<string>();

        using var reader = new StreamReader(csvStream);
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = false,
            MissingFieldFound = null,
            BadDataFound = null
        };

        using var csv = new CsvReader(reader, config);

        var isFirstRow = true;
        while (await csv.ReadAsync())
        {
            var value = csv.GetField(0);
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            value = value.Trim();

            if (isFirstRow && IsHeaderRow(value))
            {
                isFirstRow = false;
                continue;
            }

            isFirstRow = false;
            emails.Add(value);
        }

        return emails;
    }

    private static bool IsHeaderRow(string value)
    {
        var lowerValue = value.ToLowerInvariant();
        return lowerValue is "email" or "emails" or "e-mail" or "email address" or "emailaddress";
    }
}
