using MakeupClone.Application.Helpers;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllAsync(cancellationToken);
    }

    public async Task<PagedResult<Product>> GetProductsByFilterAsync(ProductFilter filter, CancellationToken cancellationToken)
    {
        var unpagedResult = await _productRepository.GetByFilterAsync(filter, cancellationToken);

        int pageSize = filter.Take;
        int pageNumber = PaginationHelper.CalculatePageNumber(filter.Skip, pageSize);

        return new PagedResult<Product>(unpagedResult.Items, unpagedResult.TotalCount, pageNumber, pageSize);
    }
}