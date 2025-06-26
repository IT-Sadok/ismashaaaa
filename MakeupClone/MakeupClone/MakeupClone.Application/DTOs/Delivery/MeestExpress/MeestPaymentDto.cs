using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.DTOs.Delivery.MeestExpress;

public class MeestPaymentDto
{
    public PayerType PaymentMethod { get; set; }

    public PayerType Payer { get; set; }
}