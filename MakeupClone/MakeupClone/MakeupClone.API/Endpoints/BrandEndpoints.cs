using MakeupClone.API.Constants;
using MakeupClone.Application.Helpers;
using MakeupClone.Application.Interfaces;

namespace MakeupClone.API.Endpoints;

public static class BrandEndpoints
{
    public static void MapBrandEndpoints(this WebApplication application)
    {
        application.MapGet(ApiRoutes.Brands.Filter, GetFilteredBrandsAsync);
    }

    private static async Task<IResult> GetFilteredBrandsAsync(IBrandService brandService, int pageNumber, int pageSize, string? sortBy, bool descending, CancellationToken cancellationToken)
    {
        var filter = FilterBuilder.Build(pageNumber, pageSize, sortBy, descending);
        var brands = await brandService.GetBrandsByFilterAsync(filter, cancellationToken);
        return Results.Ok(brands);
    }
}