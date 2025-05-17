using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface IProductService
{
    Task<ProductFilterResult> GetProductsByFilterAsync(ProductFilter filter);
}