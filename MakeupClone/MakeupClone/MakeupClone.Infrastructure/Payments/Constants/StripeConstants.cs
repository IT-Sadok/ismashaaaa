namespace MakeupClone.Infrastructure.Payments.Constants;

public static class StripeConstants
{
    public const string ProviderName = "Stripe";

    public const string PaymentMethodCard = "card";

    public const string StatusSucceeded = "succeeded";
    public const string StatusProcessing = "processing";
    public const string StatusRequiresPaymentMethod = "requires_payment_method";
    public const string StatusCanceled = "canceled";
}