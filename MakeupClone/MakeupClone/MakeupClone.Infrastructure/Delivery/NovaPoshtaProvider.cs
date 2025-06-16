using System.Globalization;
using System.Net.Http.Json;
using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.DTOs.Delivery.NovaPoshta;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery;

public class NovaPoshtaProvider : IDeliveryProvider
{
    public DeliveryType DeliveryType => DeliveryType.NovaPoshta;

    private readonly HttpClient _httpClient;
    private readonly NovaPoshtaOptions _options;

    public NovaPoshtaProvider(HttpClient httpClient, IOptions<NovaPoshtaOptions> options)
    {
        _httpClient = httpClient;
        _options = options.Value;
    }

    public async Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken)
    {
        var cityReference = await GetCityRefAsync(deliveryRequest.City, cancellationToken)
            ?? throw new InvalidOperationException($"City '{deliveryRequest.City}' not found in Nova Poshta.");

        var payload = BuildPayload("InternetDocument", "save", new CreateInternetDocumentPropertiesDto
        {
            CitySender = _options.CitySenderRef,
            CityRecipient = cityReference,
            Weight = deliveryRequest.WeightKg.ToString("F2", CultureInfo.InvariantCulture),
            ServiceType = "WarehouseWarehouse",
            PayerType = "Recipient",
            CargoType = "Parcel",
            RecipientAddress = deliveryRequest.Address,
            ContactRecipient = deliveryRequest.PhoneNumber,
            PhoneRecipient = deliveryRequest.PhoneNumber
        });

        var response = await PostAsync<InternetDocumentDataDto>(payload, cancellationToken);
        var documentNumber = response.Data.FirstOrDefault()?.InternalDocumentNumber;

        if (string.IsNullOrWhiteSpace(documentNumber))
        {
            throw new InvalidOperationException("Failed to retrieve delivery document number.");
        }

        return documentNumber;
    }

    public async Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        var payload = BuildPayload("TrackingDocument", "getStatusDocuments", new TrackingDocumentsPropertiesDto
        {
            Documents = new[] { new DocumentDto { DocumentNumber = trackingNumber } }
        });

        var response = await PostAsync<TrackingDataDto>(payload, cancellationToken);
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
        var payload = BuildPayload("Address", "getCities", new CitySearchPropertiesDto
        {
            FindByString = cityName
        });

        var response = await PostAsync<CityDataDto>(payload, cancellationToken);
        return response.Data.FirstOrDefault()?.CityReference;
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

    private async Task<NovaPoshtaResponseDto<TData>> PostAsync<TData>(object payload, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync(_options.ApiUrl, payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Nova Poshta API error: {response.StatusCode}. Content: {content}");
        }

        var result = await response.Content.ReadFromJsonAsync<NovaPoshtaResponseDto<TData>>(cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Empty or invalid response from Nova Poshta API.");

        if (!result.Success)
        {
            var errors = string.Join(", ", result.Errors.Concat(result.Warnings));
            throw new InvalidOperationException($"Nova Poshta API error: {errors}");
        }

        return result;
    }
}