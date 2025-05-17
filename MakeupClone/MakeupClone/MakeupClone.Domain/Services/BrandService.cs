using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;

namespace MakeupClone.Domain.Services;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IValidationService _validationService;

    public BrandService(IBrandRepository brandRepository, IValidationService validationService)
    {
        _brandRepository = brandRepository;
        _validationService = validationService;
    }

    public async Task<IEnumerable<Brand>> GetBrandsByFilterAsync(PagingAndSortingFilter filter)
    {
        _validationService.Validate(filter);

        return await _brandRepository.GetByFilterAsync(filter);
    }
}