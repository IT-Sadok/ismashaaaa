namespace MakeupClone.Application.Interfaces;

public interface IValidationService
{
    void ValidateAndThrow<T>(T entity);
}