using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.Interfaces;

public interface IDeliveryProvider
{
    DeliveryType DeliveryType { get; }

    Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken);

    Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(string trackingNumber, CancellationToken cancellationToken);
}