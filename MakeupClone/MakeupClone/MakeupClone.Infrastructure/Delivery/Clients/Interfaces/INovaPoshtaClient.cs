using MakeupClone.Application.DTOs.Delivery.NovaPoshta;

namespace MakeupClone.Infrastructure.Delivery.Clients.Interfaces;

public interface INovaPoshtaClient
{
    Task<NovaPoshtaResponseDto<TData>> SendRequestAsync<TPayload, TData>(NovaPoshtaRequestDto<TPayload> request, CancellationToken cancellationToken);
}