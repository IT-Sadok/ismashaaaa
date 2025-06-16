namespace MakeupClone.Application.DTOs.Delivery.NovaPoshta;

public class NovaPoshtaResponseDto<TData>
{
    public bool Success { get; set; }

    public TData[] Data { get; set; } = Array.Empty<TData>();

    public string[] Errors { get; set; } = Array.Empty<string>();

    public string[] Warnings { get; set; } = Array.Empty<string>();

    public string Information { get; set; } = string.Empty;
}
