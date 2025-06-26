using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.Services;

public class DeliveryService : IDeliveryService
{
    private readonly IReadOnlyDictionary<DeliveryType, IDeliveryProvider> _providers;

    public DeliveryService(IEnumerable<IDeliveryProvider> providers)
        => _providers = providers.ToDictionary(provider => provider.DeliveryType);

    public Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken)
    {
        var provider = GetProvider(deliveryRequest.DeliveryType);

        return provider.CreateDeliveryAsync(deliveryRequest, cancellationToken);
    }

    public Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(DeliveryType deliveryType, string trackingNumber, CancellationToken cancellationToken)
    {
        var provider = GetProvider(deliveryType);

        return provider.TrackDeliveryAsync(trackingNumber, cancellationToken);
    }

    private IDeliveryProvider GetProvider(DeliveryType deliveryType)
    {
        if (!_providers.TryGetValue(deliveryType, out var provider))
            throw new NotSupportedException($"Delivery DeliveryType {deliveryType} not supported.");

        return provider;
    }
}