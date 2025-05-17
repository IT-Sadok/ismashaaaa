using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;

namespace MakeupClone.Domain.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidationService _validationService;

    public ProductService(IProductRepository productRepository, IValidationService validationService)
    {
        _productRepository = productRepository;
        _validationService = validationService;
    }

    public async Task<ProductFilterResult> GetProductsByFilterAsync(ProductFilter filter)
    {
        _validationService.Validate(filter);

        return await _productRepository.GetByFilterAsync(filter);
    }
}