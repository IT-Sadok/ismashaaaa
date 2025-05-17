using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetCategoriesByFilterAsync(PagingAndSortingFilter filter);
}