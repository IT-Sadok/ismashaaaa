namespace MakeupClone.Infrastructure.Delivery.Clients.Interfaces;

public interface IMeestExpressClient
{
    Task<TResponse> PostAsync<TPayload, TResponse>(string controller, string method, TPayload payload, CancellationToken cancellationToken);

    Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken);
}