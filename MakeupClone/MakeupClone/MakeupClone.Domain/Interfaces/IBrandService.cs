using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface IBrandService
{
    Task<IEnumerable<Brand>> GetBrandsByFilterAsync(PagingAndSortingFilter filter);
}