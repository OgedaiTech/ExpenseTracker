namespace ExpenseTracker.Receipts.Endpoints.Create;

public class CreateReceiptRequest
{
    public Guid ExpenseId { get; set; }
    public string ReceiptNo { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public string Vendor { get; set; } = string.Empty;
}
