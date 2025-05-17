namespace MakeupClone.Domain.Filters;

public class ProductFilter : PagingAndSortingFilter
{
    public Guid? CategoryId { get; set; }

    public Guid? BrandId { get; set; }

    public string? ProductName { get; set; }

    public decimal? MinimumPrice { get; set; }

    public decimal? MaximumPrice { get; set; }
}