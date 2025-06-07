using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Interfaces;

public interface IBrandRepository
{
    Task<UnpagedResult<Brand>> GetByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken);
}