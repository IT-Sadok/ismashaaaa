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

    public async Task<IEnumerable<Category>> GetCategoriesByFilterAsync(PagingAndSortingFilter filter)
    {
        _validationService.Validate(filter);

        return await _categoryRepository.GetByFilterAsync(filter);
    }
}