using MakeupClone.Domain.Entities;

namespace MakeupClone.Application.Interfaces;

public interface IOrderService
{
    Task<Order> GetOrderByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IEnumerable<Order>> GetAllOrdersAsync(CancellationToken cancellationToken);

    Task AddOrderAsync(Order order, CancellationToken cancellationToken);

    Task UpdateOrderAsync(Order order, CancellationToken cancellationToken);

    Task DeleteOrderAsync(Guid id, CancellationToken cancellationToken);
}