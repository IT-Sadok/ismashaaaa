using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Filters;

namespace MakeupClone.API.Endpoints;

public static class BrandEndpoints
{
    public static void MapBrandEndpoints(this WebApplication app)
    {
        app.MapGet("/api/brands/filter", GetFilteredBrandsAsync);
    }

    private static async Task<IResult> GetFilteredBrandsAsync(IBrandService brandService, [AsParameters] PagingAndSortingFilter filter, CancellationToken cancellationToken)
    {
        var brands = await brandService.GetBrandsByFilterAsync(filter, cancellationToken);
        return Results.Ok(brands);
    }
}