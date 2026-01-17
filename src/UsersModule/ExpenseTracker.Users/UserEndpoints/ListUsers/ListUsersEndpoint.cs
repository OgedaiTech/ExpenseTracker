using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints.ListUsers;

internal class ListUsersEndpoint(
  UserManager<ApplicationUser> userManager
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
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    var allUsers = userManager.Users.Where(u => u.TenantId.ToString() == tenantId).ToList();

    // Prepare response
    var response = new ListUsersResponse
    {
      Users = [.. allUsers.Select(u => new UserDto
      {
        Id = Guid.Parse(u.Id),
        Email = u.Email,
        FirstName = u.FirstName,
        LastName = u.LastName,
        NationalIdentityNo = u.NationalIdentityNo,
        TaxIdNo = u.TaxIdNo,
        EmployeeId = u.EmployeeId,
        Title = u.Title
      })],
      TotalCount = allUsers.Count
    };

    await Send.OkAsync(response, cancellation: ct);
  }

}

public class ListUsersRequest
{
  public int LastId { get; set; }
  public int PageSize { get; set; } = 10;
  public string? SearchQuery { get; set; }
  public bool IsDeactivated { get; set; } = false;
}

public class ListUsersResponse
{
  public List<UserDto> Users { get; set; } = [];
  public int TotalCount { get; set; }
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
}
