using MakeupClone.API.Constants;
using MakeupClone.API.Filters;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;

namespace MakeupClone.API.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication application)
    {
        var group = application.MapGroup(ApiRoutes.Orders.Base)
            .RequireAuthorization();

        group.MapGet(ApiRoutes.Orders.GetById, GetOrderByIdAsync);
        group.MapGet(ApiRoutes.Orders.GetAll, GetAllOrdersAsync);
        group.MapPost(ApiRoutes.Orders.Create, CreateOrderAsync)
            .AddEndpointFilter<ValidationFilter<Order>>();
        group.MapPut(ApiRoutes.Orders.Update, UpdateOrderAsync)
            .AddEndpointFilter<ValidationFilter<Order>>();
        group.MapDelete(ApiRoutes.Orders.Delete, DeleteOrderAsync);
    }

    private static async Task<IResult> GetOrderByIdAsync(IOrderService orderService, Guid id, CancellationToken cancellationToken)
    {
        var order = await orderService.GetOrderByIdAsync(id, cancellationToken);
        return Results.Ok(order);
    }

    private static async Task<IResult> GetAllOrdersAsync(IOrderService orderService, CancellationToken cancellationToken)
    {
        var orders = await orderService.GetAllOrdersAsync(cancellationToken);
        return Results.Ok(orders);
    }

    private static async Task<IResult> CreateOrderAsync(IOrderProcessingService orderProcessingService, Order order, CancellationToken cancellationToken)
    {
        var orderId = await orderProcessingService.ProcessOrderAsync(order, cancellationToken);
        return Results.Created($"/api/orders/{orderId}", new { Id = orderId });
    }

    private static async Task<IResult> UpdateOrderAsync(IOrderService orderService, Order order, CancellationToken cancellationToken)
    {
        await orderService.UpdateOrderAsync(order, cancellationToken);
        return Results.NoContent();
    }

    private static async Task<IResult> DeleteOrderAsync(IOrderService orderService, Guid id, CancellationToken cancellationToken)
    {
        await orderService.DeleteOrderAsync(id, cancellationToken);
        return Results.NoContent();
    }
}