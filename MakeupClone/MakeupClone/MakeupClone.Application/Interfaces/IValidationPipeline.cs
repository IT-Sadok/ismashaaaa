namespace MakeupClone.Application.Interfaces;

public interface IValidationPipeline
{
    Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken);
}