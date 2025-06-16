namespace MakeupClone.Application.DTOs.Delivery.NovaPoshta;

public class TrackingDataDto
{
    public string Number { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? DeliveryDate { get; set; }
}
