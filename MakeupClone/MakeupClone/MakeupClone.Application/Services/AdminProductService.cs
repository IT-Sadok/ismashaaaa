using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Exceptions;

namespace MakeupClone.Application.Services;

public class AdminProductService : IAdminProductService
{
    private readonly IProductRepository _productRepository;

    public AdminProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await EnsureProductExistsAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync(CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllAsync(cancellationToken);
    }

    public async Task AddProductAsync(Product product, CancellationToken cancellationToken)
    {
        await _productRepository.AddAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateProductAsync(Product product, CancellationToken cancellationToken)
    {
        await EnsureProductExistsAsync(product.Id, cancellationToken);
        await _productRepository.UpdateAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken)
    {
        await EnsureProductExistsAsync(id, cancellationToken);

        await _productRepository.DeleteAsync(id, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task AddDiscountAsync(Guid id, decimal discountPercentage, CancellationToken cancellationToken)
    {
        var product = await EnsureProductExistsAsync(id, cancellationToken);

        product.ApplyDiscount(discountPercentage);

        await _productRepository.UpdateAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveDiscountAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await EnsureProductExistsAsync(id, cancellationToken);

        product.RemoveDiscount();

        await _productRepository.UpdateAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDiscountAsync(Guid id, decimal newDiscountPercentage, CancellationToken cancellationToken)
    {
        var product = await EnsureProductExistsAsync(id, cancellationToken);

        product.UpdateDiscount(newDiscountPercentage);

        await _productRepository.UpdateAsync(product, cancellationToken);
        await _productRepository.SaveChangesAsync(cancellationToken);
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