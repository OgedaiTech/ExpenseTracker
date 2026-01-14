using FastEndpoints;
using Microsoft.AspNetCore.Identity;

namespace ExpenseTracker.Users.UserEndpoints.CreateNewUser;

internal class CreateWithPassword(UserManager<ApplicationUser> userManager) : Endpoint<CreateUserRequest, EmptyResponse>
{
  public override void Configure()
  {
    Post("/users");
    Roles("SystemAdmin", "TenantAdmin");
  }

  public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
  {
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;
    if (string.IsNullOrEmpty(req.Email) || string.IsNullOrEmpty(req.Password))
    {
      AddError("EMAIL_AND_PASSWORD_ARE_REQUIRED");
      ThrowIfAnyErrors();
    }
    var newUser = new ApplicationUser
    {
      UserName = req.Email,
      Email = req.Email,
      TenantId = Guid.Parse(tenantId),
      FirstName = req.FirstName,
      LastName = req.LastName,
      NationalIdentityNo = req.NationalIdentityNo,
      TaxIdNo = req.TaxIdNo,
      EmployeeId = req.EmployeeId,
      Title = req.Title,
      IsDeactivated = false
    };

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
