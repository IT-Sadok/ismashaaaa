namespace MakeupClone.Application.DTOs.Delivery.UkrPoshta;

public class UkrPoshtaTrackResponseDto
{
    public string Status { get; set; } = null!;

    public DateTime? EstimatedDeliveryDate { get; set; }
}