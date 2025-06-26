using MakeupClone.Application.DTOs.Delivery;
using MakeupClone.Domain.Enums;

namespace MakeupClone.Tests.Builders;

public static class DeliveryRequestBuilder
{
    public static DeliveryRequestDto BuildDefaultRequest(
        string city = "Київ",
        string address = "вул. Шевченка, 1",
        string recipientsPhoneNumber = "+380954282549",
        double weight = 1,
        decimal price = 100,
        string recipientName = "Іван Петров",
        PaymentType paymentType = PaymentType.Visa,
        PayerType payerType = PayerType.Recipient)
    {
        return new DeliveryRequestDto
        {
            City = city,
            Address = address,
            RecipientsPhoneNumber = recipientsPhoneNumber,
            WeightKg = weight,
            DeclaredPrice = price,
            RecipientName = recipientName,
            PaymentMethod = paymentType,
            Payer = payerType
        };
    }
}