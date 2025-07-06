namespace MakeupClone.Application.DTOs.Delivery.NovaPoshta;

public class CreateInternetDocumentPropertiesDto
{
    public string CitySender { get; set; } = null!;

    public string Sender { get; set; } = null!;

    public string SenderAddress { get; set; } = null!;

    public string ContactSender { get; set; } = null!;

    public string SendersPhone { get; set; } = null!;

    public string RecipientCityRef { get; set; } = null!;

    public string RecipientWarehouseRef { get; set; } = null!;

    public string RecipientName { get; set; } = null!;

    public string RecipientContactName { get; set; } = null!;

    public string RecipientsPhone { get; set; } = null!;

    public string RecipientAddress { get; set; } = null!;

    public string ServiceType { get; set; } = "WarehouseWarehouse";

    public string PayerType { get; set; } = "Recipient";

    public string PaymentMethod { get; set; } = "Visa";

    public string CargoType { get; set; } = "Parcel";

    public string Weight { get; set; } = null!;

    public decimal Cost { get; set; }

    public string SeatsAmount { get; set; } = "1";

    public string Description { get; set; } = null!;
}