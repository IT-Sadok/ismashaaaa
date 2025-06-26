namespace MakeupClone.Application.DTOs.Delivery.NovaPoshta;

public class NovaPoshtaResponseDto<TData>
{
    public bool Success { get; set; }

    public TData[] Data { get; set; } =[];

    public string[] Errors { get; set; } =[];

    public string[] Warnings { get; set; } =[];

    public string Information { get; set; } = string.Empty;
}
