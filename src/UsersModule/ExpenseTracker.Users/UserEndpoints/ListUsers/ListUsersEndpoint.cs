using FastEndpoints;

namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

internal class ListUsersEndpoint(
  IListUsersService service
) : Endpoint<ListUsersRequest, ListUsersResponse>
{
  public override void Configure()
  {
    Get("/users");
    Roles("SystemAdmin", "TenantAdmin");
  }

  public override async Task HandleAsync(ListUsersRequest req, CancellationToken ct)
  {
    // Extract tenant ID from claims
    var tenantId = Guid.Parse(User.Claims.First(x => x.Type == "TenantId").Value);

    var serviceResult = await service.ListUsersAsync(
      tenantId,
      req.Cursor,
      req.PageSize,
      req.SearchQuery,
      req.IsDeactivated,
      ct);

    if (!serviceResult.Success)
    {
      await Send.ErrorsAsync(statusCode: 400, cancellation: ct);
      return;
    }

    var response = new ListUsersResponse
    {
      Users = [.. serviceResult.Data!.Users.Select(u => new UserDto
      {
        Id = Guid.Parse(u.Id),
        Email = u.Email,
        FirstName = u.FirstName,
        LastName = u.LastName,
        NationalIdentityNo = u.NationalIdentityNo,
        TaxIdNo = u.TaxIdNo,
        EmployeeId = u.EmployeeId,
        Title = u.Title,
        IsDeactivated = u.IsDeactivated
      })],
      NextCursor = serviceResult.Data.NextCursor,
      HasMore = serviceResult.Data.HasMore,
      PageSize = req.PageSize
    };

    await Send.OkAsync(response, ct);
  }

}

public class ListUsersRequest
{
  public string? Cursor { get; set; }
  public int PageSize { get; set; } = 2;
  public string? SearchQuery { get; set; }
  public bool? IsDeactivated { get; set; }
}

public class ListUsersResponse
{
  public List<UserDto> Users { get; set; } = [];
  public string? NextCursor { get; set; }
  public bool HasMore { get; set; }
  public int PageSize { get; set; }
}

public class UserDto
{
  public Guid Id { get; set; }
  public string? Email { get; set; } = string.Empty;
  public string? FirstName { get; set; } = string.Empty;
  public string? LastName { get; set; } = string.Empty;
  public string? NationalIdentityNo { get; set; } = string.Empty;
  public string? TaxIdNo { get; set; } = string.Empty;
  public string? EmployeeId { get; set; } = string.Empty;
  public string? Title { get; set; } = string.Empty;
  public bool IsDeactivated { get; set; }
}
