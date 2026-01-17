using System.ComponentModel.DataAnnotations;

namespace ExpenseTrackerUI.Services.User;

public class CreateUserWithInvitationDto
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email format")]
    public string Email { get; set; } = string.Empty;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? NationalIdentityNo { get; set; }

    public string? TaxIdNo { get; set; }

    public string? EmployeeId { get; set; }

    public string? Title { get; set; }
}

public record CreateUserWithInvitationResponse(
    string UserId,
    string Email,
    bool InvitationSent,
    string? Message = null);
