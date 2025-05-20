using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;

namespace MakeupClone.Domain.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidationService _validationService;

    public CategoryService(ICategoryRepository categoryRepository, IValidationService validationService)
    {
        _categoryRepository = categoryRepository;
        _validationService = validationService;
    }

    public async Task<PagedResult<Category>> GetCategoriesByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        _validationService.ValidateAndThrow(filter);

        var (items, totalCount) = await _categoryRepository.GetByFilterAsync(filter, cancellationToken);

        int pageSize = filter.Take;
        int pageNumber = CalculatePageNumber(filter.Skip, pageSize);

        return new PagedResult<Category>(items, totalCount, pageNumber, pageSize);
    }

    private static int CalculatePageNumber(int skip, int take)
    {
        return (skip / take) + 1;
    }
}