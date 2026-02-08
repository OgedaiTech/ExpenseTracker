using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Email.Contracts;
using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ExpenseTracker.Users.UserEndpoints.CreateNewUser;

internal partial class CreateUserWithInvitation(
    UserManager<ApplicationUser> userManager,
    IMediator mediator,
    ILogger<CreateUserWithInvitation> logger)
    : Endpoint<CreateUserWithInvitationRequest, CreateUserWithInvitationResponse>
{
  public override void Configure()
  {
    Post("/users/invite");
    Roles("SystemAdmin", "TenantAdmin");
  }

  public override async Task HandleAsync(CreateUserWithInvitationRequest req, CancellationToken ct)
  {
    // Validate email is provided
    if (string.IsNullOrWhiteSpace(req.Email))
    {
      var problem = Results.Problem(
        detail: "Email is required",
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
    }

    // Validate email format
    if (!IsValidEmail(req.Email))
    {
      var problem = Results.Problem(
        detail: "Invalid email format",
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
    }

    var email = req.Email.Trim();

    // Check if user already exists
    var existingUser = await userManager.FindByEmailAsync(email);
    if (existingUser != null)
    {
      var problem = Results.Problem(
        title: "Invalid request",
        detail: "A user with this email already exists",
        statusCode: StatusCodes.Status400BadRequest,
        instance: HttpContext.Request.Path);

      await Send.ResultAsync(problem);
    }

    // Extract tenant ID from claims
    var tenantId = User.Claims.First(x => x.Type == "TenantId").Value;

    // Create new user without password
    var newUser = new ApplicationUser
    {
      UserName = email,
      Email = email,
      TenantId = Guid.Parse(tenantId),
      FirstName = req.FirstName,
      LastName = req.LastName,
      NationalIdentityNo = req.NationalIdentityNo,
      TaxIdNo = req.TaxIdNo,
      EmployeeId = req.EmployeeId,
      Title = req.Title,
      IsDeactivated = false
    };

    // Create user without password
    var createResult = await userManager.CreateAsync(newUser);
    if (!createResult.Succeeded)
    {
      LogUserCreationFailed(logger, email, string.Join(", ", createResult.Errors.Select(e => e.Description)));
      var errors = string.Join("; ", createResult.Errors.Select(e => e.Description));
      var problem = Results.Problem(
        detail: errors,
        statusCode: StatusCodes.Status500InternalServerError,
        instance: HttpContext.Request.Path);
      await Send.ResultAsync(problem);
    }

    // Generate password reset token
    var token = await userManager.GeneratePasswordResetTokenAsync(newUser);

    // Attempt to send invitation email
    bool invitationSent;
    string? warningMessage = null;

    var emailCommand = new SendInvitationEmailCommand(email, token);
    var emailResult = await mediator.Send(emailCommand, ct);

    if (emailResult.Success)
    {
      invitationSent = true;
      LogInvitationSentSuccessfully(logger, email, newUser.Id);
    }
    else
    {
      invitationSent = false;
      warningMessage = "User created but invitation email failed to send";
      LogEmailSendFailed(logger, email, emailResult.Message ?? "UNKNOWN_ERROR", null);
    }

    var response = new CreateUserWithInvitationResponse(
        UserId: newUser.Id,
        Email: email,
        InvitationSent: invitationSent,
        Message: warningMessage
    );

    await Send.OkAsync(response, ct);
  }

  private static bool IsValidEmail(string email)
  {
    return new EmailAddressAttribute().IsValid(email);
  }

  [LoggerMessage(
      EventId = 200,
      Level = LogLevel.Warning,
      Message = "Failed to create user {Email}: {Error}")]
  private static partial void LogUserCreationFailed(ILogger logger, string email, string error);

  [LoggerMessage(
      EventId = 201,
      Level = LogLevel.Information,
      Message = "User {Email} created successfully with ID {UserId}. Invitation email sent.")]
  private static partial void LogInvitationSentSuccessfully(ILogger logger, string email, string userId);

  [LoggerMessage(
      EventId = 202,
      Level = LogLevel.Warning,
      Message = "Failed to send invitation email to {Email}: {Error}")]
  private static partial void LogEmailSendFailed(ILogger logger, string email, string error, Exception? exception);
}
