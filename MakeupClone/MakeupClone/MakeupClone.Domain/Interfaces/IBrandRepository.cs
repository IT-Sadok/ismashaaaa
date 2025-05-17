using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Domain.Interfaces;

public interface IBrandRepository
{
    Task<IEnumerable<Brand>> GetByFilterAsync(PagingAndSortingFilter filter);
}