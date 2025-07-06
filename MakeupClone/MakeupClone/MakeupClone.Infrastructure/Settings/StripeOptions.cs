namespace MakeupClone.Infrastructure.Settings;

public class StripeOptions
{
    public string SecretKey { get; set; } = default!;

    public string PublicKey { get; set; } = default!;
}