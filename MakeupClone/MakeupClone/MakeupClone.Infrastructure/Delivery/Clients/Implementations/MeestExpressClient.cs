using Flurl;
using Flurl.Http;
using MakeupClone.Application.DTOs.Delivery.MeestExpress;
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

    public async Task<CreateMeestDeliveryResponseDto> CreateDeliveryAsync(CreateMeestDeliveryRequestDto payload, CancellationToken cancellationToken)
    {
        var response = await _options.ApiUrl
            .AppendPathSegments(DeliveryApiConstants.ApiSegment, DeliveryApiConstants.ShipmentsSegment)
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .WithHeader(DeliveryApiConstants.HeaderAuthorization, $"{DeliveryApiConstants.AuthorizationSchemeBearer}{_options.ApiKey}")
            .PostJsonAsync(payload, cancellationToken: cancellationToken)
            .ReceiveJson<CreateMeestDeliveryResponseDto>();

        return response;
    }

    public async Task<TrackMeestDeliveryResponseDto> GetDeliveryDetailsAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        var url = _options.ApiUrl
           .AppendPathSegments(DeliveryApiConstants.ApiSegment, DeliveryApiConstants.ShipmentsSegment, trackingNumber, DeliveryApiConstants.TrackingSegment);

        return await url
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .WithHeader(DeliveryApiConstants.HeaderAuthorization, $"{DeliveryApiConstants.AuthorizationSchemeBearer}{_options.ApiKey}")
            .GetJsonAsync<TrackMeestDeliveryResponseDto>(cancellationToken: cancellationToken);
    }
}