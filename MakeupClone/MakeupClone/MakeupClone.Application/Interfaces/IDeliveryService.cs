using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.Interfaces;

public interface IDeliveryService
{
    Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken);

    Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(DeliveryType deliveryType, string trackingNumber, CancellationToken cancellationToken);
}