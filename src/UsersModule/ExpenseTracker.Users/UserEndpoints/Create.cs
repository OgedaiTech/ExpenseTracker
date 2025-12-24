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
    if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password) || req.TenantId == Guid.Empty)
    {
      AddError("EMAIL_AND_PASSWORD_TENANTID_ARE_REQUIRED");
      ThrowIfAnyErrors();
    }
    var newUser = new ApplicationUser { UserName = req.Email, Email = req.Email, TenantId = req.TenantId };

    var result = await userManager.CreateAsync(newUser, req.Password);

    if (!result.Succeeded)
    {
      foreach (var error in result.Errors)
      {
        AddError(error.Description);
      }
      ThrowIfAnyErrors();
    }

    await Send.OkAsync(ct);
  }
}
