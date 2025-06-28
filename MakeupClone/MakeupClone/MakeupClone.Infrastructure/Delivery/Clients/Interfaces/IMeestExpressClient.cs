using MakeupClone.Application.DTOs.Delivery.MeestExpress;

namespace MakeupClone.Infrastructure.Delivery.Clients.Interfaces;

public interface IMeestExpressClient
{
    Task<CreateMeestDeliveryResponseDto> CreateDeliveryAsync(CreateMeestDeliveryRequestDto payload, CancellationToken cancellationToken);

    Task<TrackMeestDeliveryResponseDto> GetDeliveryDetailsAsync(string trackingNumber, CancellationToken cancellationToken);
}