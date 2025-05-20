using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface IBrandRepository
{
    Task<(IEnumerable<Brand> Items, int TotalCount)> GetByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken);
}