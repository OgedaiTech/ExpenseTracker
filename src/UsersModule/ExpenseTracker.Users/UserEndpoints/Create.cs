using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints;

internal class Create(UserManager<ApplicationUser> userManager) : Endpoint<CreateUserRequest, EmptyResponse>
{
  public override void Configure()
  {
    Post("/users");
    Roles("SystemAdmin", "TenantAdmin");
  }

  public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
  {
    var newUser = new ApplicationUser { UserName = req.Email, Email = req.Email };

    await userManager.CreateAsync(newUser, req.Password);

    await Send.OkAsync(ct);
  }
}
