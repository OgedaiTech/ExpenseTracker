namespace ExpenseTrackerUI.Services.User;

public class RoleUpdateResult
{
    public Guid UserId { get; set; }
    public bool IsApprover { get; set; }
}
