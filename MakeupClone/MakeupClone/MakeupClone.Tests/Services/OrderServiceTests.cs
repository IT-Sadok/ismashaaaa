using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;
using MakeupClone.Domain.Exceptions;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.Entities;
using MakeupClone.Tests.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Tests.Services;

public class OrderServiceTests : IAsyncLifetime
{
    private readonly MakeupCloneDbContext _dbContext;
    private readonly IOrderRepository _orderRepository;
    private readonly OrderService _orderService;
    private readonly ServiceProvider _serviceProvider;

    public OrderServiceTests()
    {
        _serviceProvider = TestServiceProviderFactory.Create();
        _dbContext = _serviceProvider.GetRequiredService<MakeupCloneDbContext>();
        _orderRepository = _serviceProvider.GetRequiredService<IOrderRepository>();

        _orderService = new OrderService(_orderRepository);
    }

    public async Task InitializeAsync()
    {
        await InitializeDataAsync(_dbContext);
    }

    public Task DisposeAsync()
    {
        TestDbContextFactory.ClearDatabase(_dbContext);

        return Task.CompletedTask;
    }

    private async Task InitializeDataAsync(MakeupCloneDbContext dbContext)
    {
        var user = new User
        {
            Id = Guid.NewGuid().ToString(),
            UserName = "existingOrder@example.com",
            Email = "existingOrder@example.com",
            FirstName = "Existing",
            LastName = "User",
            BirthDate = DateTime.UtcNow.AddYears(-25),
            ReceiveNotifications = true
        };
        dbContext.Users.Add(user);

        var orders = new[]
        {
            new OrderEntity
            {
                Id = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                UserId = user.Id,
                User = user,
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Pending,
                Items = new List<OrderItemEntity>()
            },
            new OrderEntity
            {
                Id = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                UserId = user.Id,
                User = user,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                Status = OrderStatus.Delivered,
                Items = new List<OrderItemEntity>()
            }
        };
        dbContext.Orders.AddRange(orders);
        await dbContext.SaveChangesAsync();
    }

    [Fact]
    public async Task GetOrderByIdAsync_WithExistingId_ShouldReturnOrder()
    {
        var existingOrderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        var result = await _orderService.GetOrderByIdAsync(existingOrderId, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(existingOrderId, result.Id);
        Assert.Equal(OrderStatus.Pending, result.Status);
    }

    [Fact]
    public async Task GetOrderByIdAsync_WithNonExistingId_ShouldThrowNotFound()
    {
        var orderId = Guid.NewGuid();

        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _orderService.GetOrderByIdAsync(orderId, CancellationToken.None));

        Assert.Equal($"Order with ID {orderId} not found.", exception.Message);
    }

    [Fact]
    public async Task GetAllOrdersAsync_ShouldReturnAllOrders()
    {
        var result = await _orderService.GetAllOrdersAsync(CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task AddOrderAsync_WithValidOrder_ShouldAddSuccessfully()
    {
        var newOrder = new Order
        {
            Id = Guid.NewGuid(),
            UserId = "test-user",
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Items = new List<OrderItem>()
        };

        await _orderService.AddOrderAsync(newOrder, CancellationToken.None);

        var result = await _dbContext.Orders.FindAsync(newOrder.Id);

        Assert.NotNull(result);
        Assert.Equal("test-user", result.UserId);
        Assert.Equal(OrderStatus.Pending, result.Status);
    }

    [Fact]
    public async Task UpdateOrderAsync_WithExistingOrder_ShouldUpdateSuccessfully()
    {
        var existingOrder = await _dbContext.Orders.FirstAsync();

        var updatedOrder = new Order
        {
            Id = existingOrder.Id,
            UserId = existingOrder.UserId,
            CreatedAt = existingOrder.CreatedAt,
            Status = OrderStatus.Shipped,
            Items = new List<OrderItem>()
        };

        await _orderService.UpdateOrderAsync(updatedOrder, CancellationToken.None);

        var result = await _dbContext.Orders.FindAsync(updatedOrder.Id);

        Assert.NotNull(result);
        Assert.Equal(OrderStatus.Shipped, result.Status);
    }

    [Fact]
    public async Task UpdateOrderAsync_WithNonExistingOrder_ShouldThrowNotFound()
    {
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = "user-123",
            CreatedAt = DateTime.UtcNow,
            Status = OrderStatus.Cancelled,
            Items = new List<OrderItem>()
        };

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _orderService.UpdateOrderAsync(order, CancellationToken.None));

        Assert.Equal($"Order with ID {order.Id} not found.", result.Message);
    }

    [Fact]
    public async Task DeleteOrderAsync_WithValidId_ShouldDeleteSuccessfully()
    {
        var orderId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");

        await _orderService.DeleteOrderAsync(orderId, CancellationToken.None);

        var result = await _dbContext.Orders.FindAsync(orderId);

        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteOrderAsync_WithNonExistingId_ShouldThrowNotFound()
    {
        var orderId = Guid.NewGuid();

        var result = await Assert.ThrowsAsync<EntityNotFoundException>(() =>
            _orderService.DeleteOrderAsync(orderId, CancellationToken.None));

        Assert.Equal($"Order with ID {orderId} not found.", result.Message);
    }
}