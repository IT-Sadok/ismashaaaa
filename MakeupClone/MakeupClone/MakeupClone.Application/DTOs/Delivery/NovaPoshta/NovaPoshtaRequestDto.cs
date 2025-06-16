namespace MakeupClone.Application.DTOs.Delivery.NovaPoshta;

public class NovaPoshtaRequestDto<TProperties>
{
    public string ApiKey { get; set; } = null!;

    public string ModelName { get; set; } = null!;

    public string CalledMethod { get; set; } = null!;

    public TProperties MethodProperties { get; set; } = default!;
}