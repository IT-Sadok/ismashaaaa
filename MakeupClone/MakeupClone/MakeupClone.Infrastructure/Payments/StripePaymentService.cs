using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Payments.Constants;
using MakeupClone.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Stripe;

namespace MakeupClone.Infrastructure.Payments;

public class StripePaymentService : IPaymentService
{
    private readonly PaymentIntentService _paymentIntentService;

    public StripePaymentService(IOptions<StripeOptions> stripeOptions, PaymentIntentService paymentIntentService)
    {
        StripeConfiguration.ApiKey = stripeOptions.Value.SecretKey;
        _paymentIntentService = paymentIntentService;
    }

    public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency, PaymentType paymentMethod, CancellationToken cancellationToken)
    {
        var stripeMethod = MapPaymentMethod(paymentMethod);

        var options = new PaymentIntentCreateOptions
        {
            Amount = ConvertToStripeAmount(amount),
            Currency = currency.ToLowerInvariant(),
            PaymentMethodTypes = new List<string> { stripeMethod }
        };

        var intent = await _paymentIntentService.CreateAsync(options, cancellationToken: cancellationToken);
        return intent.ClientSecret;
    }

    public async Task<PaymentStatus> ConfirmPaymentAsync(string paymentIntentId, CancellationToken cancellationToken)
    {
        try
        {
            var intent = await _paymentIntentService.GetAsync(paymentIntentId, cancellationToken: cancellationToken);
            return MapStripeStatusToPaymentStatus(intent.Status);
        }
        catch (StripeException)
        {
            return PaymentStatus.Failed;
        }
    }

    public string GetProviderName() => StripeConstants.ProviderName;

    private static string MapPaymentMethod(PaymentType paymentMethod) =>
        paymentMethod switch
        {
            PaymentType.Visa => StripeConstants.PaymentMethodCard,
            PaymentType.MasterCard => StripeConstants.PaymentMethodCard,
            PaymentType.GooglePay => StripeConstants.PaymentMethodCard,
            PaymentType.ApplePay => StripeConstants.PaymentMethodCard,
            _ => throw new ArgumentOutOfRangeException(nameof(paymentMethod), $"Unsupported payment method: {paymentMethod}")
        };

    private static PaymentStatus MapStripeStatusToPaymentStatus(string status) =>
        status switch
        {
            StripeConstants.StatusSucceeded => PaymentStatus.Completed,
            StripeConstants.StatusProcessing => PaymentStatus.Pending,
            StripeConstants.StatusRequiresPaymentMethod => PaymentStatus.Failed,
            StripeConstants.StatusCanceled => PaymentStatus.Failed,
            _ => PaymentStatus.Pending
        };

    private static long ConvertToStripeAmount(decimal amount) => (long)(amount * 100);
}