using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.DTOs.Delivery.UkrPoshta;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery;

public class UkrPoshtaProvider : IDeliveryProvider
{
    public DeliveryType DeliveryType => DeliveryType.UkrPoshta;

    private readonly IUkrPoshtaClient _ukrPoshtaClient;
    private readonly UkrPoshtaOptions _options;

    public UkrPoshtaProvider(IUkrPoshtaClient ukrPoshtaClient, IOptions<UkrPoshtaOptions> options)
    {
        _ukrPoshtaClient = ukrPoshtaClient;
        _options = options.Value;
    }

    public async Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken)
    {
        var payload = BuildPayload(deliveryRequest);

        var response = await _ukrPoshtaClient.CreateDeliveryAsync(payload, cancellationToken);

        if (string.IsNullOrWhiteSpace(response.Barcode))
            throw new InvalidOperationException("Failed to receive barcode from UkrPoshta.");

        return response.Barcode;
    }

    public async Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        var response = await _ukrPoshtaClient.GetDeliveryDetailsAsync(trackingNumber, cancellationToken);

        return new DeliveryTrackingInformationDto
        {
            TrackingNumber = trackingNumber,
            Status = response.Status,
            EstimatedDeliveryDate = response.EstimatedDeliveryDate
        };
    }

    private CreateUkrPoshtaDeliveryPropertiesDto BuildPayload(DeliveryRequestDto deliveryRequest)
    {
        return new CreateUkrPoshtaDeliveryPropertiesDto
        {
            SenderAddressId = _options.SenderAddressId,
            Recipient = new UkrPoshtaRecipientDto
            {
                Name = deliveryRequest.RecipientName,
                PhoneNumber = deliveryRequest.RecipientsPhoneNumber
            },
            Address = new UkrPoshtaAddressDto
            {
                City = deliveryRequest.City,
                Street = deliveryRequest.Address
            },
            Parcel = new UkrPoshtaParcelDto
            {
                Weight = ConvertKgToGrams(deliveryRequest.WeightKg),
                DeclaredPrice = (double)deliveryRequest.DeclaredPrice
            },
            PaymentMethod = (PaymentType)deliveryRequest.PaymentMethod
        };
    }

    private int ConvertKgToGrams(double weightKg) => (int)(weightKg * 1000);
}