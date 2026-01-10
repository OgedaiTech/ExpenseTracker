using Microsoft.AspNetCore.Http;

namespace ExpenseTracker.Users.UserEndpoints.BulkCreate;

public class BulkCreateUsersRequest
{
    public IFormFile CsvFile { get; set; } = null!;
}
