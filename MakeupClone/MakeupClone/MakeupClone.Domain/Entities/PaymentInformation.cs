using MakeupClone.Domain.Enums;

namespace MakeupClone.Domain.Entities;

public class PaymentInformation
{
    public Guid Id { get; set; }

    public PaymentType Method { get; set; }

    public decimal Amount { get; set; }

    public DateTime PaidAt { get; set; } = DateTime.UtcNow;

    public string TransactionId { get; set; }

    public string Provider { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public Order Order { get; set; }
}