using MakeupClone.API.Constants;
using MakeupClone.API.Filters;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;

namespace MakeupClone.API.Endpoints;

public static class AdminProductEndpoints
{
    public static void MapAdminProductEndpoints(this WebApplication application)
    {
        var group = application.MapGroup(ApiRoutes.AdminProducts.Base)
            .RequireAuthorization("AdminPolicy");

        group.MapGet(ApiRoutes.AdminProducts.GetById, GetProductByIdAsync);
        group.MapGet(ApiRoutes.AdminProducts.GetAll, GetAllProductsAsync);
        group.MapPost(ApiRoutes.AdminProducts.Create, AddProductAsync)
            .AddEndpointFilter<ValidationFilter<Product>>();
        group.MapPut(ApiRoutes.AdminProducts.Update, UpdateProductAsync)
            .AddEndpointFilter<ValidationFilter<Product>>();
        group.MapDelete(ApiRoutes.AdminProducts.Delete, DeleteProductAsync);

        group.MapPost(ApiRoutes.AdminProducts.AddDiscount, AddDiscountAsync);
        group.MapPut(ApiRoutes.AdminProducts.UpdateDiscount, UpdateDiscountAsync);
        group.MapDelete(ApiRoutes.AdminProducts.RemoveDiscount, RemoveDiscountAsync);
    }

    private static async Task<IResult> GetProductByIdAsync(IAdminProductService adminProductService, Guid id, CancellationToken cancellationToken)
    {
        var product = await adminProductService.GetProductByIdAsync(id, cancellationToken);
        return Results.Ok(product);
    }

    private static async Task<IResult> GetAllProductsAsync(IAdminProductService adminProductService, CancellationToken cancellationToken)
    {
        var products = await adminProductService.GetAllProductsAsync(cancellationToken);
        return Results.Ok(products);
    }

    private static async Task<IResult> AddProductAsync(IAdminProductService adminProductService, Product product, CancellationToken cancellationToken)
    {
         await adminProductService.AddProductAsync(product, cancellationToken);
        return Results.Created($"/api/admin/products/{product.Id}", product);
    }

    private static async Task<IResult> UpdateProductAsync(IAdminProductService adminProductService, Product product, CancellationToken cancellationToken)
    {
        await adminProductService.UpdateProductAsync(product, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteProductAsync(IAdminProductService adminProductService, Guid id, CancellationToken cancellationToken)
    {
        await adminProductService.DeleteProductAsync(id, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> AddDiscountAsync(IAdminProductService adminProductService, Guid id, decimal discountPercentage, CancellationToken cancellationToken)
    {
        await adminProductService.AddDiscountAsync(id, discountPercentage, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> UpdateDiscountAsync(IAdminProductService adminProductService, Guid id, decimal discountPercentage, CancellationToken cancellationToken)
    {
        await adminProductService.UpdateDiscountAsync(id, discountPercentage, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> RemoveDiscountAsync(IAdminProductService adminProductService, Guid id, CancellationToken cancellationToken)
    {
        await adminProductService.RemoveDiscountAsync(id, cancellationToken);
        return Results.NoContent();
    }
}