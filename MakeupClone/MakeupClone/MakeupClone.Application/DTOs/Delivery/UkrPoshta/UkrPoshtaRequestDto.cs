namespace MakeupClone.Application.DTOs.Delivery.UkrPoshta;

public class UkrPoshtaRequestDto<TProperties>
{
    public TProperties Data { get; set; } = default!;
}