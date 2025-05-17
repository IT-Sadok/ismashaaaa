using MakeupClone.Domain.Entities;

namespace MakeupClone.Domain.Filters;

public class ProductFilterResult
{
    public IEnumerable<Product> Products { get; set; }

    public int TotalCount { get; set; }
}