using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IValidationService _validationService;

    public BrandService(IBrandRepository brandRepository, IValidationService validationService)
    {
        _brandRepository = brandRepository;
        _validationService = validationService;
    }

    public async Task<PagedResult<Brand>> GetBrandsByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(filter);

        var (items, totalCount) = await _brandRepository.GetByFilterAsync(filter, cancellationToken);

        int pageSize = filter.Take;
        int pageNumber = CalculatePageNumber(filter.Skip, pageSize);

        return new PagedResult<Brand>(items, totalCount, pageNumber, pageSize);
    }

    private static int CalculatePageNumber(int skip, int take)
    {
        return (skip / take) + 1;
    }
}