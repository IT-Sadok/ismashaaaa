using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.DTOs.Delivery;

public class DeliveryRequestDto
{
    public DeliveryType DeliveryType { get; set; }

    public string City { get; set; } = default!;

    public string Address { get; set; } = default!;

    public string RecipientsPhoneNumber { get; set; } = default!;

    public double WeightKg { get; set; }

    public string? SendersPhoneNumber { get; set; } = default!;

    public string? Region { get; set; }

    public string? PostalCode { get; set; }

    public string? RecipientName { get; set; }

    public decimal? DeclaredPrice { get; set; }

    public PaymentType? PaymentMethod { get; set; }

    public string? DeliveryPaymentMethod { get; set; }

    public PayerType? Payer { get; set; }

    public string? SeatsAmount { get; set; }

    public string? Description { get; set; }
}