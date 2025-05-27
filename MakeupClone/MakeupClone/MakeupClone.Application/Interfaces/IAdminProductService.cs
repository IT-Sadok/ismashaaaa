using MakeupClone.Domain.Entities;

namespace MakeupClone.Application.Interfaces;

public interface IAdminProductService
{
    Task<Product> GetProductByIdAsync(Guid id, CancellationToken cancellationToken);

    Task AddProductAsync(Product product, CancellationToken cancellationToken);

    Task UpdateProductAsync(Product product, CancellationToken cancellationToken);

    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken);
}