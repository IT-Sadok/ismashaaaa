namespace MakeupClone.Domain.Interfaces;

public interface IValidationService
{
    void ValidateAndThrow<T>(T entity);
}