namespace MakeupClone.Application.DTOs.Delivery.NovaPoshta;

public class CreateInternetDocumentPropertiesDto
{
    public string CitySender { get; set; } = null!;

    public string CityRecipient { get; set; } = null!;

    public string Weight { get; set; } = null!;

    public string ServiceType { get; set; } = null!;

    public string PayerType { get; set; } = null!;

    public string CargoType { get; set; } = null!;

    public string RecipientAddress { get; set; } = null!;

    public string ContactRecipient { get; set; } = null!;

    public string PhoneRecipient { get; set; } = null!;
}
