using Flurl.Http;
using MakeupClone.Application.DTOs.Delivery.NovaPoshta;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Delivery.Constants;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery.Clients.Implementations;
public class NovaPoshtaClient : INovaPoshtaClient
{
    private readonly NovaPoshtaOptions _options;

    public NovaPoshtaClient(IOptions<NovaPoshtaOptions> options)
    {
        _options = options.Value;
    }

    public async Task<NovaPoshtaResponseDto<TData>> SendRequestAsync<TPayload, TData>(NovaPoshtaRequestDto<TPayload> request, CancellationToken cancellationToken)
    {
        var response = await _options.ApiUrl
            .WithHeader(DeliveryApiConstants.HeaderContentType, DeliveryApiConstants.ContentTypeJson)
            .PostJsonAsync(request, cancellationToken: cancellationToken)
            .ReceiveJson<NovaPoshtaResponseDto<TData>>();

        if (!response.Success)
        {
            var errors = string.Join(", ", response.Errors.Concat(response.Warnings));
            throw new InvalidOperationException($"Nova Poshta API error: {errors}");
        }

        return response;
    }
}