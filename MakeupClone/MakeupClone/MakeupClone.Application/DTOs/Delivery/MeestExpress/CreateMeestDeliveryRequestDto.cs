namespace MakeupClone.Application.DTOs.Delivery.MeestExpress;

public class CreateMeestDeliveryRequestDto
{
    public string SenderCode { get; set; } = null!;

    public MeestRecipientDto Recipient { get; set; } = null!;

    public MeestAddressDto Address { get; set; } = null!;

    public MeestParcelDto Parcel { get; set; } = null!;

    public MeestPaymentDto Payment { get; set; } = null!;
}