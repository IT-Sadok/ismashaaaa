namespace MakeupClone.Application.DTOs.Delivery.MeestExpress;

public class TrackMeestDeliveryResponseDto
{
    public string Status { get; set; } = null!;

    public DateTime? EstimatedDeliveryDate { get; set; }
}