namespace MakeupClone.Application.DTOs.Delivery;

public class DeliveryTrackingInformationDto
{
    public string TrackingNumber { get; set; } = default!;

    public string Status { get; set; } = default!;

    public DateTime? EstimatedDeliveryDate { get; set; }
}