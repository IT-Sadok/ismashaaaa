namespace MakeupClone.Infrastructure.Settings;

public class NovaPoshtaOptions
{
    public string ApiUrl { get; set; } = default!;

    public string ApiKey { get; set; } = default!;

    public string CitySenderRef { get; set; } = null!;

    public string SenderRef { get; set; } = null!;

    public string SenderContactRef { get; set; } = null!;

    public string SenderWarehouseRef { get; set; } = null!;
}