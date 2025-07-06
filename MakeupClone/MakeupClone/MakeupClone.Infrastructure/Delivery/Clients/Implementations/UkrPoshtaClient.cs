using Flurl;
using Flurl.Http;
using MakeupClone.Application.DTOs.Delivery.UkrPoshta;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Delivery.Constants;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery.Clients.Implementations;

public class UkrPoshtaClient : IUkrPoshtaClient
{
    private readonly UkrPoshtaOptions _options;

    public UkrPoshtaClient(IOptions<UkrPoshtaOptions> options)
    {
        _options = options.Value;
    }

    public async Task<UkrPoshtaCreateDeliveryResponseDto> CreateDeliveryAsync(CreateUkrPoshtaDeliveryPropertiesDto payload, CancellationToken cancellationToken)
    {
        var response = await _options.ApiUrl
            .AppendPathSegments(DeliveryApiConstants.BackofficeSegment, DeliveryApiConstants.ShipmentsSegment)
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .WithHeader(DeliveryApiConstants.HeaderXApiKey, _options.ApiKey)
            .PostJsonAsync(payload, cancellationToken: cancellationToken)
            .ReceiveJson<UkrPoshtaCreateDeliveryResponseDto>();

        return response;
    }

    public async Task<UkrPoshtaTrackResponseDto> GetDeliveryDetailsAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        var url = _options.ApiUrl
            .AppendPathSegments(DeliveryApiConstants.BackofficeSegment, DeliveryApiConstants.ShipmentsSegment, trackingNumber, DeliveryApiConstants.TrackingSegment);

        return await url
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .WithHeader(DeliveryApiConstants.HeaderXApiKey, _options.ApiKey)
            .GetJsonAsync<UkrPoshtaTrackResponseDto>(cancellationToken: cancellationToken);
    }
}