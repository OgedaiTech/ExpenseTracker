using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.UpdateUser;

internal partial class UpdateUserEndpoint(
    UserManager<ApplicationUser> userManager,
    ILogger<UpdateUserEndpoint> logger) : Endpoint<UpdateUserRequest, UpdateUserResponse>
{
  public override void Configure()
  {
    Put("/users/{userId:guid}");
    Roles("SystemAdmin", "TenantAdmin");
  }

  public override async Task HandleAsync(UpdateUserRequest req, CancellationToken ct)
  {
    var userId = Route<Guid>("userId");
    var user = await userManager.FindByIdAsync(userId.ToString());

    if (user is null)
    {
      var problem = Results.Problem(
          detail: $"No user found with ID {userId}",
          statusCode: StatusCodes.Status404NotFound,
          instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      LogUserNotFound(logger, userId);
      return;
    }

    // Ensure tenant admins can only update users in their tenant
    var requestingUsersTenantId = User.Claims.First(c => c.Type == "TenantId").Value;
    if (user.TenantId.ToString() != requestingUsersTenantId && !User.IsInRole("SystemAdmin"))
    {
      var problem = Results.Problem(
          detail: "You do not have permission to update this user",
          statusCode: StatusCodes.Status403Forbidden,
          instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
      LogCannotUpdateUserFromDifferentTenant(logger, userId);
      return;
    }

    // Update user fields
    user.FirstName = req.FirstName;
    user.LastName = req.LastName;
    user.NationalIdentityNo = req.NationalIdentityNo;
    user.TaxIdNo = req.TaxIdNo;
    user.EmployeeId = req.EmployeeId;
    user.Title = req.Title;

    var result = await userManager.UpdateAsync(user);

    if (!result.Succeeded)
    {
      var errors = string.Join("; ", result.Errors.Select(e => e.Description));
      var problem = Results.Problem(
          detail: errors,
          statusCode: StatusCodes.Status500InternalServerError,
          instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
    }

    var response = new UpdateUserResponse
    {
      Id = Guid.Parse(user.Id),
      Email = user.Email,
      FirstName = user.FirstName,
      LastName = user.LastName,
      NationalIdentityNo = user.NationalIdentityNo,
      TaxIdNo = user.TaxIdNo,
      EmployeeId = user.EmployeeId,
      Title = user.Title,
      IsDeactivated = user.IsDeactivated
    };

    LogUserUpdated(logger, userId);
    await Send.OkAsync(response, ct);
  }

  [LoggerMessage(EventId = 220, Level = LogLevel.Warning, Message = "User not found when trying to update: {UserId}")]
  private static partial void LogUserNotFound(ILogger logger, Guid userId);

  [LoggerMessage(EventId = 221, Level = LogLevel.Warning, Message = "Cannot update user from different tenant: {UserId}")]
  private static partial void LogCannotUpdateUserFromDifferentTenant(ILogger logger, Guid userId);

  [LoggerMessage(EventId = 222, Level = LogLevel.Information, Message = "User updated: {UserId}")]
  private static partial void LogUserUpdated(ILogger logger, Guid userId);
}
