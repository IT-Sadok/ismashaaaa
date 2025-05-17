using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface IProductRepository
{
    Task<ProductFilterResult> GetByFilterAsync(ProductFilter filter);
}