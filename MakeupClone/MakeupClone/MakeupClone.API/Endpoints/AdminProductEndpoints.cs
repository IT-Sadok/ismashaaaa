using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Interfaces;

namespace MakeupClone.API.Endpoints;

public static class AdminProductEndpoints
{
    public static void MapAdminProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/admin/products")
            .RequireAuthorization("AdminPolicy");

        group.MapGet("/{id}", GetProductByIdAsync);
        group.MapPost(" ", AddProductAsync);
        group.MapPut("/{id}", UpdateProductAsync);
        group.MapDelete("/{id}", DeleteProductAsync);
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

    private static async Task<IResult> UpdateProductAsync(Guid id, Product product, IAdminProductService adminProductService, CancellationToken cancellationToken)
    {
        await adminProductService.UpdateProductAsync(product, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteProductAsync(IAdminProductService adminProductService, Guid id, CancellationToken cancellationToken)
    {
        await adminProductService.DeleteProductAsync(id, cancellationToken);
        return Results.NoContent();
    }
}