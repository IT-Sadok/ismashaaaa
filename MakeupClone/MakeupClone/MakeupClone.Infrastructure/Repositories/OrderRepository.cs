using AutoMapper;
using AutoMapper.QueryableExtensions;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace MakeupClone.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly MakeupCloneDbContext _dbContext;
    private readonly IMapper _mapper;

    public OrderRepository(MakeupCloneDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<Order> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var order = await _dbContext.Orders
            .Include(order => order.Items)
            .ThenInclude(orderItem => orderItem.Product)
            .AsNoTracking()
            .FirstOrDefaultAsync(order => order.Id == id, cancellationToken);

        return _mapper.Map<Order>(order);
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Orders
           .Include(order => order.Items)
           .ThenInclude(orderItem => orderItem.Product)
           .ProjectTo<Order>(_mapper.ConfigurationProvider)
           .AsNoTracking()
           .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken)
    {
        var orderEntity = _mapper.Map<OrderEntity>(order);

        await _dbContext.Orders.AddAsync(orderEntity, cancellationToken);
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken)
    {
        var existingOrder = await _dbContext.Orders
            .Include(orderDb => orderDb.Items)
            .FirstOrDefaultAsync(orders => orders.Id == order.Id, cancellationToken);

        if (existingOrder == null)
            return;

        _mapper.Map(order, existingOrder);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var existingOrder = await _dbContext.Orders.FindAsync(id, cancellationToken);

        if (existingOrder == null)
            return;

        _dbContext.Orders.Remove(existingOrder);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
