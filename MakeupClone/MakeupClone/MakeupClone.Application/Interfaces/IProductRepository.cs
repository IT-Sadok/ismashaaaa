using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Interfaces;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken);

    Task AddAsync(Product product, CancellationToken cancellationToken);

    Task UpdateAsync(Product product, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<UnpagedResult<Product>> GetByFilterAsync(ProductFilter filter, CancellationToken cancellationToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);
}