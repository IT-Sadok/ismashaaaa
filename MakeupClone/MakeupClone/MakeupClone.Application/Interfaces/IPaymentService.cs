using MakeupClone.Domain.Enums;

namespace MakeupClone.Application.Interfaces;

public interface IPaymentService
{
    Task<string> CreatePaymentIntentAsync(decimal amount, string currency, PaymentType paymentMethod, CancellationToken cancellationToken);

    Task<PaymentStatus> ConfirmPaymentAsync(string paymentIntentId, CancellationToken cancellationToken);
}