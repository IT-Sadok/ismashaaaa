using MakeupClone.API.Constants;
using MakeupClone.API.Filters;
using MakeupClone.Application.Helpers;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Filters;

namespace MakeupClone.API.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this WebApplication application)
    {
        application.MapGet(ApiRoutes.Categories.Filter, GetFilteredCategoriesAsync)
            .AddEndpointFilter<ValidationFilter<PagingAndSortingFilter>>();
    }

    private static async Task<IResult> GetFilteredCategoriesAsync(ICategoryService categoryService, int pageNumber, int pageSize, string? sortBy, bool descending, CancellationToken cancellationToken)
    {
        var filter = FilterBuilder.Build(pageNumber, pageSize, sortBy, descending);
        var categories = await categoryService.GetCategoriesByFilterAsync(filter, cancellationToken);
        return Results.Ok(categories);
    }
}