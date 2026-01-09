using System.Net;

namespace ExpenseTrackerUI.Services;

public class ServiceResult<T>
{
  public bool IsSuccess { get; set; }
  public T? Data { get; set; }
  public HttpStatusCode StatusCode { get; set; }
  public string? ErrorCode { get; set; }
  public string? ErrorMessage { get; set; }

  public static ServiceResult<T> Success(T data) => new()
  {
    IsSuccess = true,
    Data = data,
    StatusCode = HttpStatusCode.Created
  };

  public static ServiceResult<T> Failure(HttpStatusCode statusCode, string? errorCode, string? message) => new()
  {
    IsSuccess = false,
    StatusCode = statusCode,
    ErrorCode = errorCode,
    ErrorMessage = message ?? "An error occurred"
  };
}
