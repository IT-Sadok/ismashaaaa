using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.DTOs.Delivery.UkrPoshta;

public class CreateUkrPoshtaDeliveryPropertiesDto
{
    public string SenderAddressId { get; set; } = null!;

    public UkrPoshtaRecipientDto Recipient { get; set; } = null!;

    public UkrPoshtaAddressDto Address { get; set; } = null!;

    public UkrPoshtaParcelDto Parcel { get; set; } = null!;

    public PaymentType PaymentMethod { get; set; }
}