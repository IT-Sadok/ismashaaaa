using Flurl;
using Flurl.Http;
using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Application.DTOs.Delivery.MeestExpress;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Delivery.Constants;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace MakeupClone.Infrastructure.Delivery;

public class MeestExpressProvider : IDeliveryProvider
{
    public DeliveryType DeliveryType => DeliveryType.MeestExpress;

    private readonly IMeestExpressClient _meestExpressClient;
    private readonly MeestExpressOptions _options;

    public MeestExpressProvider(IMeestExpressClient meestExpressClient, IOptions<MeestExpressOptions> options)
    {
        _meestExpressClient = meestExpressClient;
        _options = options.Value;
    }

    public async Task<string> CreateDeliveryAsync(DeliveryRequestDto deliveryRequest, CancellationToken cancellationToken)
    {
        var payload = BuildPayload(deliveryRequest);

        var deliveryResponse = await _meestExpressClient.PostAsync<CreateMeestDeliveryRequestDto, CreateMeestDeliveryResponseDto>(DeliveryApiConstants.ApiSegment, DeliveryApiConstants.ShipmentsSegment, payload, cancellationToken);

        if (string.IsNullOrWhiteSpace(deliveryResponse.Barcode))
            throw new InvalidOperationException("Failed to receive barcode from Meest Express.");

        return deliveryResponse.Barcode;
    }

    public async Task<DeliveryTrackingInformationDto> TrackDeliveryAsync(string trackingNumber, CancellationToken cancellationToken)
    {
        var url = _options.ApiUrl
            .AppendPathSegments(DeliveryApiConstants.ApiSegment, DeliveryApiConstants.ShipmentsSegment, trackingNumber, DeliveryApiConstants.TrackingSegment);

        var deliveryResponse = await _meestExpressClient.GetAsync<TrackMeestDeliveryResponseDto>(url, cancellationToken);

        return new DeliveryTrackingInformationDto
        {
            TrackingNumber = trackingNumber,
            Status = deliveryResponse.Status,
            EstimatedDeliveryDate = deliveryResponse.EstimatedDeliveryDate
        };
    }

    private CreateMeestDeliveryRequestDto BuildPayload(DeliveryRequestDto deliveryRequest)
    {
        return new CreateMeestDeliveryRequestDto
        {
            SenderCode = _options.SenderCode,
            Recipient = new MeestRecipientDto
            {
                Name = deliveryRequest.RecipientName!,
                Phone = deliveryRequest.RecipientsPhoneNumber
            },
            Address = new MeestAddressDto
            {
                City = deliveryRequest.City,
                Street = deliveryRequest.Address
            },
            Parcel = new MeestParcelDto
            {
                WeightKg = deliveryRequest.WeightKg,
                DeclaredPrice = (decimal)deliveryRequest.DeclaredPrice,
            },
            Payment = new MeestPaymentDto
            {
                PaymentMethod = (PayerType)deliveryRequest.PaymentMethod,
                Payer = deliveryRequest.Payer ?? PayerType.Recipient
            }
        };
    }
}