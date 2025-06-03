using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;

namespace MakeupClone.API.Endpoints;

public static class AdminProductEndpoints
{
    public static void MapAdminProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/admin/products")
            .RequireAuthorization("AdminPolicy");

        group.MapGet("/{id}", GetProductByIdAsync);
        group.MapPost("/", AddProductAsync);
        group.MapPut("/{id}", UpdateProductAsync);
        group.MapDelete("/{id}", DeleteProductAsync);
        group.MapPost("/{id}/discount", AddDiscountAsync);
        group.MapPut("/{id}/discount", UpdateDiscountAsync);
        group.MapDelete("/{id}/discount", RemoveDiscountAsync);
    }

    private static async Task<IResult> GetProductByIdAsync(IAdminProductService adminProductService, Guid id, CancellationToken cancellationToken)
    {
        var product = await adminProductService.GetProductByIdAsync(id, cancellationToken);
        return Results.Ok(product);
    }

    private static async Task<IResult> AddProductAsync(IAdminProductService adminProductService, Product product, CancellationToken cancellationToken)
    {
         await adminProductService.AddProductAsync(product, cancellationToken);
        return Results.Created($"/api/admin/products/{product.Id}", product);
    }

    private static async Task<IResult> UpdateProductAsync(IAdminProductService adminProductService, Guid id, Product product, CancellationToken cancellationToken)
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