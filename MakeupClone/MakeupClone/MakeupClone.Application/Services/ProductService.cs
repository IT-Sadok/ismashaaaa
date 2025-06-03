using MakeupClone.Application.Helpers;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidationService _validationService;

    public ProductService(IProductRepository productRepository, IValidationService validationService)
    {
        _productRepository = productRepository;
        _validationService = validationService;
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllAsync(cancellationToken);
    }

    public async Task<PagedResult<Product>> GetProductsByFilterAsync(ProductFilter filter, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(filter);

        var (items, totalCount) = await _productRepository.GetByFilterAsync(filter, cancellationToken);

        int pageSize = filter.Take;
        int pageNumber = PaginationHelper.CalculatePageNumber(filter.Skip, pageSize);

        return new PagedResult<Product>(items, totalCount, pageNumber, pageSize);
    }
}