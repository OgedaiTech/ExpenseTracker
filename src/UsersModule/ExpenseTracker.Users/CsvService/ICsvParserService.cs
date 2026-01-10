namespace ExpenseTracker.Users.CsvService;

public interface ICsvParserService
{
    Task<List<string>> ParseEmailsAsync(Stream csvStream, CancellationToken cancellationToken);
}
