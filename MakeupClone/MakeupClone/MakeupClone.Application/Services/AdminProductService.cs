using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Exceptions;

namespace MakeupClone.Application.Services;

public class AdminProductService : IAdminProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IValidationService _validationService;

    public AdminProductService(IProductRepository productRepository, IValidationService validationService)
    {
        _productRepository = productRepository;
        _validationService = validationService;
    }

    public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(id);

        return await EnsureProductExistsAsync(id, cancellationToken);
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(product);

        await _productRepository.AddAsync(product, cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(product);

        await EnsureProductExistsAsync(product.Id, cancellationToken);
        await _productRepository.UpdateAsync(product, cancellationToken);
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(id);

        await EnsureProductExistsAsync(id, cancellationToken);
        await _productRepository.DeleteAsync(id, cancellationToken);
    }

    private async Task<Product> EnsureProductExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(id, cancellationToken);
        if (product == null)
        {
            throw new EntityNotFoundException($"Product with ID {id} not found.");
        }

        return product;
    }
}