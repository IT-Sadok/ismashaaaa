using System.Globalization;
using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.DTOs.Delivery.NovaPoshta;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Delivery.Constants;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery;

public class NovaPoshtaProvider : IDeliveryProvider
{
    public DeliveryType DeliveryType => DeliveryType.NovaPoshta;

    private readonly INovaPoshtaClient _novaPoshtaClient;
    private readonly NovaPoshtaOptions _options;

    public NovaPoshtaProvider(INovaPoshtaClient novaPoshtaClient, IOptions<NovaPoshtaOptions> options)
    {
        _novaPoshtaClient = novaPoshtaClient;
        _options = options.Value;
    }

    public async Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken)
    {
        var cityReference = await GetCityRefAsync(deliveryRequest.City, cancellationToken)
            ?? throw new InvalidOperationException($"City '{deliveryRequest.City}' not found in Nova Poshta.");

        var warehouseReference = await GetWarehouseRefAsync(cityReference, deliveryRequest.Address, cancellationToken)
            ?? throw new InvalidOperationException($"Address '{deliveryRequest.Address}' not found in Nova Poshta.");

        var payload = BuildPayload(NovaPoshtaConstants.ModelInternetDocument, NovaPoshtaConstants.MethodSave, new CreateInternetDocumentPropertiesDto
        {
            CitySender = _options.CitySenderRef,
            Sender = _options.SenderRef,
            SenderAddress = _options.SenderWarehouseRef,
            ContactSender = _options.SenderContactRef,
            SendersPhone = deliveryRequest.SendersPhoneNumber,
            RecipientCityRef = cityReference,
            RecipientWarehouseRef = warehouseReference,
            RecipientName = deliveryRequest.RecipientName,
            RecipientContactName = deliveryRequest.RecipientName!,
            RecipientsPhone = deliveryRequest.RecipientsPhoneNumber,
            PayerType = deliveryRequest.Payer.ToString(),
            PaymentMethod = deliveryRequest.DeliveryPaymentMethod,
            Weight = deliveryRequest.WeightKg.ToString("F2", CultureInfo.InvariantCulture),
            ServiceType = NovaPoshtaConstants.ServiceTypeWarehouseToWarehouse,
            CargoType = NovaPoshtaConstants.CargoTypeParcel,
            Cost = (decimal)deliveryRequest.DeclaredPrice,
            SeatsAmount = deliveryRequest.SeatsAmount,
            Description = deliveryRequest.Description
        });

        var response = await _novaPoshtaClient.SendRequestAsync<CreateInternetDocumentPropertiesDto, InternetDocumentDataDto>(payload, cancellationToken);

        var documentNumber = response.Data.FirstOrDefault()?.InternalDocumentNumber;

        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new InvalidOperationException("Failed to retrieve delivery document number.");

        return documentNumber;
    }

    public async Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        var payload = BuildPayload(NovaPoshtaConstants.ModelTrackingDocument, NovaPoshtaConstants.MethodGetStatusDocuments, new TrackingDocumentsPropertiesDto
        {
            Documents = new[] { new DocumentDto { DocumentNumber = trackingNumber } }
        });

        var response = await _novaPoshtaClient.SendRequestAsync<TrackingDocumentsPropertiesDto, TrackingDataDto>(payload, cancellationToken);
        var data = response.Data.FirstOrDefault()
            ?? throw new InvalidOperationException($"Tracking information not found for '{trackingNumber}'.");

        return new DeliveryTrackingInformationDto
        {
            TrackingNumber = data.Number,
            Status = data.Status,
            EstimatedDeliveryDate = data.DeliveryDate
        };
    }

    private async Task<string?> GetCityRefAsync(string cityName, CancellationToken cancellationToken)
    {
        var payload = BuildPayload(NovaPoshtaConstants.ModelAddress, NovaPoshtaConstants.MethodGetCities, new CitySearchPropertiesDto
        {
            FindByString = cityName
        });

        var response = await _novaPoshtaClient.SendRequestAsync<CitySearchPropertiesDto, CityDataDto>(payload, cancellationToken);
        return response.Data.FirstOrDefault()?.CityReference;
    }

    private async Task<string?> GetWarehouseRefAsync(string cityReference, string address, CancellationToken cancellationToken)
    {
        var payload = BuildPayload(NovaPoshtaConstants.ModelAddress, NovaPoshtaConstants.MethodGetWarehouses, new WarehouseSearchPropertiesDto
        {
            CityRef = cityReference
        });

        var response = await _novaPoshtaClient.SendRequestAsync<WarehouseSearchPropertiesDto, WarehouseDataDto>(payload, cancellationToken);

        return response.Data
            .FirstOrDefault(warehouseDataDto => warehouseDataDto.Description.Contains(address, StringComparison.OrdinalIgnoreCase))
            ?.Ref;
    }

    private NovaPoshtaRequestDto<T> BuildPayload<T>(string modelName, string method, T methodProperties)
    {
        return new NovaPoshtaRequestDto<T>
        {
            ApiKey = _options.ApiKey,
            ModelName = modelName,
            CalledMethod = method,
            MethodProperties = methodProperties
        };
    }
}