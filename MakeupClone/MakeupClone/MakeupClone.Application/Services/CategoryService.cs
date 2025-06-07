using MakeupClone.Application.Helpers;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Common;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Filters;

namespace MakeupClone.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IValidationPipeline _validationPipeline;

    public CategoryService(ICategoryRepository categoryRepository, IValidationPipeline validationPipeline)
    {
        _categoryRepository = categoryRepository;
        _validationPipeline = validationPipeline;
    }

    public async Task<PagedResult<Category>> GetCategoriesByFilterAsync(PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        await _validationPipeline.ExecuteAsync(filter, cancellationToken);

        var unpagedResult = await _categoryRepository.GetByFilterAsync(filter, cancellationToken);

        int pageSize = filter.Take;
        int pageNumber = PaginationHelper.CalculatePageNumber(filter.Skip, pageSize);

        return new PagedResult<Category>(unpagedResult.Items, unpagedResult.TotalCount, pageNumber, pageSize);
    }
}