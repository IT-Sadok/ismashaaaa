using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface ICategoryService
{
    Task<PagedResult<Category>> GetCategoriesByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken);
}