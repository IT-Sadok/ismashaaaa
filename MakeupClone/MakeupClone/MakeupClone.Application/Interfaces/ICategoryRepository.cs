using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Interfaces;

public interface ICategoryRepository
{
    Task<UnpagedResult<Category>> GetByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken);
}