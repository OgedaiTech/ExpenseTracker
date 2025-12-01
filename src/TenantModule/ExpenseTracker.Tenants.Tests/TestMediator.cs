using ExpenseTracker.Core;
using ExpenseTracker.Tenants.Endpoints.Create;
using MediatR;

namespace ExpenseTracker.Tenants.Tests;

public class TestMediator : IMediator
{
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
      return Task.FromResult((TResponse)(object)new ServiceResult());
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
