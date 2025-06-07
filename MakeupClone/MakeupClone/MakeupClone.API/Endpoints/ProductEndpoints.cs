using MakeupClone.API.Constants;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Filters;

namespace MakeupClone.API.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication application)
    {
        application.MapGet(ApiRoutes.Products.GetAll, GetAllProductsAsync);
        application.MapGet(ApiRoutes.Products.Filter, GetFilteredProductsAsync);
    }

    private static async Task<IResult> GetAllProductsAsync(IAdminProductService adminProductService, CancellationToken cancellationToken)
    {
        var products = await adminProductService.GetAllProductsAsync(cancellationToken);
        return Results.Ok(products);
    }

    private static async Task<IResult> GetFilteredProductsAsync(
        IProductService productService,
        int pageNumber,
        int pageSize,
        string? sortBy,
        bool descending,
        Guid? categoryId,
        Guid? brandId,
        string? productName,
        decimal? minimumPrice,
        decimal? maximumPrice,
        CancellationToken cancellationToken)
    {
        var filter = new ProductFilter
        {
            Skip = (pageNumber - 1) * pageSize,
            Take = pageSize,
            OrderBy = sortBy,
            Descending = descending,
            CategoryId = categoryId,
            BrandId = brandId,
            ProductName = productName,
            MinimumPrice = minimumPrice,
            MaximumPrice = maximumPrice
        };
        var filteredResult = await productService.GetProductsByFilterAsync(filter, cancellationToken);
        return Results.Ok(filteredResult);
    }
}