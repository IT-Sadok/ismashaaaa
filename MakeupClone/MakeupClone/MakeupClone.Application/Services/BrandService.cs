using MakeupClone.Application.Helpers;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<PagedResult<Brand>> GetBrandsByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        var unpagedResult = await _brandRepository.GetByFilterAsync(filter, cancellationToken);

        int pageSize = filter.Take;
        int pageNumber = PaginationHelper.CalculatePageNumber(filter.Skip, pageSize);

        return new PagedResult<Brand>(unpagedResult.Items, unpagedResult.TotalCount, pageNumber, pageSize);
    }
}