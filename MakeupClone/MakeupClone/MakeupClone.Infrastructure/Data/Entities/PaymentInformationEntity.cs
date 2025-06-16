using MakeupClone.Domain.Enums;

namespace MakeupClone.Infrastructure.Data.Entities;

public class PaymentInformationEntity
{
    public Guid Id { get; set; }

    public PaymentType Method { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; }

    public string TransactionId { get; set; }

    public string Provider { get; set; }

    public PaymentStatus Status { get; set; }

    public OrderEntity Order { get; set; }
}