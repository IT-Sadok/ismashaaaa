using FluentValidation;
using MakeupClone.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Application.Services;

public class ValidationPipeline : IValidationPipeline
{
    private readonly IServiceProvider _serviceProvider;

    public ValidationPipeline(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task ExecuteAsync<TRequest>(TRequest request, CancellationToken cancellationToken)
    {
        var validator = _serviceProvider.GetService<IValidator<TRequest>>();

        if (validator == null)
            return;

        var result = await validator.ValidateAsync(request, cancellationToken);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);
    }
}