using Flurl;
using Flurl.Http;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Delivery.Constants;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery.Clients.Implementations;

public class MeestExpressClient : IMeestExpressClient
{
    private readonly MeestExpressOptions _options;

    public MeestExpressClient(IOptions<MeestExpressOptions> options)
    {
        _options = options.Value;
    }

    public async Task<TResponse> PostAsync<TPayload, TResponse>(string controller, string method, TPayload payload, CancellationToken cancellationToken)
    {
        var response = await _options.ApiUrl
            .AppendPathSegments(controller, method)
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .WithHeader(DeliveryApiConstants.HeaderAuthorization, $"{DeliveryApiConstants.AuthorizationSchemeBearer}{_options.ApiKey}")
            .PostJsonAsync(payload, cancellationToken: cancellationToken)
            .ReceiveJson<TResponse>();

        return response;
    }

    public async Task<TResponse> GetAsync<TResponse>(string url, CancellationToken cancellationToken)
    {
        var response = await url
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .WithHeader(DeliveryApiConstants.HeaderAuthorization, $"{DeliveryApiConstants.AuthorizationSchemeBearer}{_options.ApiKey}")
            .GetJsonAsync<TResponse>(cancellationToken: cancellationToken);

        return response;
    }
}