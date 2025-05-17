namespace MakeupClone.Domain.Interfaces;

public interface IValidationService
{
    void Validate<T>(T entity);
}