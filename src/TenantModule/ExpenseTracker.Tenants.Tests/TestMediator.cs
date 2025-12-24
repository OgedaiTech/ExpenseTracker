using ExpenseTracker.Core;
using ExpenseTracker.Tenants.Endpoints.Create;
using MediatR;

namespace ExpenseTracker.Tenants.Tests;

public class TestMediator : IMediator
{
  public bool ShouldReturnError { get; set; }

  public IAsyncEnumerable<TResponse> CreateStream<TResponse>(IStreamRequest<TResponse> request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public IAsyncEnumerable<object?> CreateStream(object request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task Publish(object notification, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }

  public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
  {
    throw new NotImplementedException();
  }

  public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
  {
    if (request is CreateTenantAdminUserCommand)
    {
      var result = ShouldReturnError ? new ServiceResult("ERROR") : new ServiceResult();
      return Task.FromResult((TResponse)(object)result);
    }

    throw new NotImplementedException("Unhandled request in test mediator");
  }

  public Task Send<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
  {
    throw new NotImplementedException();
  }

  public Task<object?> Send(object request, CancellationToken cancellationToken = default)
  {
    throw new NotImplementedException();
  }
}
