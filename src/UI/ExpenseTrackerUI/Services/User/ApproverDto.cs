namespace ExpenseTrackerUI.Services.User;

public class ApproverDto
{
  public Guid Id { get; set; }
  public string FirstName { get; set; } = string.Empty;
  public string LastName { get; set; } = string.Empty;
  public string Email { get; set; } = string.Empty;
  public string FullName => $"{FirstName} {LastName}".Trim();
  public bool IsFavorite { get; set; }
}
