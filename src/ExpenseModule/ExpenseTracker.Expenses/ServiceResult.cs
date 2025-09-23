namespace ExpenseTracker.Expenses;

public class ServiceResult
{
  public string? Message { get; set; }
  public bool Success { get; set; }

  public ServiceResult()
  {
    Success = true;
  }

  public ServiceResult(string message)
  {
    Message = message;
    Success = false;
  }
}

public class ServiceResult<T> : ServiceResult
{
  public T? Data { get; set; }

  public ServiceResult(string message, T? data = default) : base(message)
  {
    Data = data;
  }
}
