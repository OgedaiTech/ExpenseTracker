using System.Text.Json.Serialization;

namespace ExpenseTrackerUI.Services;

internal sealed class ProblemDetailsResponse
{
  [JsonPropertyName("type")]
  public string? Type { get; set; }

  [JsonPropertyName("title")]
  public string? Title { get; set; }

  [JsonPropertyName("status")]
  public int? Status { get; set; }

  [JsonPropertyName("detail")]
  public string? Detail { get; set; }

  [JsonPropertyName("instance")]
  public string? Instance { get; set; }
}
