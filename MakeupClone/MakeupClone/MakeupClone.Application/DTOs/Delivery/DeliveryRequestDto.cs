using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.DTOs.Delivery;

public class DeliveryRequestDto
{
    public DeliveryType deliveryType { get; set; }

    public string City { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string PhoneNumber { get; set; } = default!;

    public double WeightKg { get; set; }
}