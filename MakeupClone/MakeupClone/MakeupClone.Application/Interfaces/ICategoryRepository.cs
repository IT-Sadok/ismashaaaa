using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Interfaces;

public interface ICategoryRepository
{
    Task<(IEnumerable<Category> Items, int TotalCount)> GetByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken);
}