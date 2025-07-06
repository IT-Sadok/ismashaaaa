using MakeupClone.Domain.Entities;

namespace MakeupClone.Application.Interfaces;

public interface IOrderProcessingService
{
    Task<Guid> ProcessOrderAsync(Order order, CancellationToken cancellationToken);
}