using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface IProductService
{
    Task<PagedResult<Product>> GetProductsByFilterAsync(ProductFilter filter, CancellationToken cancellationToken);
}