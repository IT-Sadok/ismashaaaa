using MakeupClone.Application.DTOs.Delivery.UkrPoshta;

namespace MakeupClone.Infrastructure.Delivery.Clients.Interfaces;

public interface IUkrPoshtaClient
{
    Task<UkrPoshtaCreateDeliveryResponseDto> CreateDeliveryAsync(CreateUkrPoshtaDeliveryPropertiesDto payload, CancellationToken cancellationToken);

    Task<UkrPoshtaTrackResponseDto> GetDeliveryDetailsAsync(string trackingNumber, CancellationToken cancellationToken);
}