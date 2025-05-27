using FluentValidation;
using MakeupClone.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Application.Services;

public class ValidationService : IValidationService
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void ValidateAndThrow<T>(T entity)
    {
        var validator = _serviceProvider.GetService<IValidator<T>>();
        if (validator == null)
        {
            throw new InvalidOperationException($"No validator found for type {typeof(T).Name}.");
        }

        var result = validator.Validate(entity);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
    }
}