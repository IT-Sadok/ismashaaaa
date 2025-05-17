using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetByFilterAsync(PagingAndSortingFilter filter);
}