using System.Text.Json.Serialization;

namespace ExpenseTracker.Accounting.Providers.Parasut;

public class ParasutPurchaseBillRequest
{
  [JsonPropertyName("data")]
  public ParasutPurchaseBillData Data { get; set; } = new();
}

public class ParasutPurchaseBillData
{
  [JsonPropertyName("type")]
  public string Type { get; set; } = "purchase_bills";

  [JsonPropertyName("attributes")]
  public ParasutPurchaseBillAttributes Attributes { get; set; } = new();
}

public class ParasutPurchaseBillAttributes
{
  [JsonPropertyName("description")]
  public string Description { get; set; } = string.Empty;

  [JsonPropertyName("item_type")]
  public string ItemType { get; set; } = "purchase_bill";

  [JsonPropertyName("issue_date")]
  public string IssueDate { get; set; } = string.Empty;

  [JsonPropertyName("due_date")]
  public string DueDate { get; set; } = string.Empty;

  [JsonPropertyName("currency")]
  public string Currency { get; set; } = "TRL";

  [JsonPropertyName("net_total")]
  public decimal NetTotal { get; set; }

  [JsonPropertyName("total_vat")]
  public decimal TotalVat { get; set; }

  [JsonPropertyName("details_attributes")]
  public List<ParasutPurchaseBillDetailAttributes> DetailsAttributes { get; set; } = [];
}

public class ParasutPurchaseBillDetailAttributes
{
  [JsonPropertyName("description")]
  public string Description { get; set; } = string.Empty;

  [JsonPropertyName("quantity")]
  public int Quantity { get; set; } = 1;

  [JsonPropertyName("unit_price")]
  public decimal UnitPrice { get; set; }
}
