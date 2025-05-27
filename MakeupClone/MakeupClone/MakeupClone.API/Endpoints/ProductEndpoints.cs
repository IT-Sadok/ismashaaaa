using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Filters;

namespace MakeupClone.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        app.MapGet("/api/products/filter", GetFilteredProductsAsync);
    }

    private static async Task<IResult> GetFilteredProductsAsync(IProductService productService, [AsParameters] ProductFilter filter, CancellationToken cancellationToken)
    {
        var filteredResult = await productService.GetProductsByFilterAsync(filter, cancellationToken);
        return Results.Ok(filteredResult);
    }
}