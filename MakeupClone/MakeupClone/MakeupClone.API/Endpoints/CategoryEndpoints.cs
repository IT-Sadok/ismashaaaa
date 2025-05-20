using MakeupClone.Domain.Filters;
using MakeupClone.Domain.Interfaces;

namespace MakeupClone.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication app)
    {
        app.MapGet("/api/categories/filter", GetFilteredCategoriesAsync);
    }

    private static async Task<IResult> GetFilteredCategoriesAsync(ICategoryService categoryService, [AsParameters] PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        var categories = await categoryService.GetCategoriesByFilterAsync(filter, cancellationToken);
        return Results.Ok(categories);
    }
}