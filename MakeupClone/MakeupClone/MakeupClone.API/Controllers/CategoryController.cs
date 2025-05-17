using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakeupClone.API.Controllers;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetFilteredCategories([FromQuery] PagingAndSortingFilter baseFilter)
    {
        var categories = await _categoryService.GetCategoriesByFilterAsync(baseFilter);

        return Ok(categories);
    }
}