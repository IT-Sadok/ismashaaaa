using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Exceptions;

namespace MakeupClone.Application.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;

    public OrderService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await EnsureOrderExistsAsync(id, cancellationToken);
    }

    public async Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken cancellationToken)
    {
        return await _orderRepository.GetAllAsync(cancellationToken);
    }

    public async Task AddOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await _orderRepository.AddAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateOrderAsync(Order order, CancellationToken cancellationToken)
    {
        await EnsureOrderExistsAsync(order.Id, cancellationToken);
        await _orderRepository.UpdateAsync(order, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken)
    {
        await EnsureOrderExistsAsync(id, cancellationToken);

        await _orderRepository.DeleteAsync(id, cancellationToken);
        await _orderRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Order> EnsureOrderExistsAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _orderRepository.GetByIdAsync(id, cancellationToken);
        if (order == null)
        {
            throw new EntityNotFoundException($"Order with ID {id} not found.");
        }

        return order;
    }
}