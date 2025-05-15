using FluentValidation;
using MakeupClone.Application.DTOs;
using MakeupClone.Application.Validators;

namespace MakeupClone.API.Extensions;

public static class FluentValidatorConfigurationExtensions
{
    public static IServiceCollection AddCustomValidators(this IServiceCollection services)
    {
        services.AddTransient<IValidator<LoginDto>, LoginValidator>();
        services.AddTransient<IValidator<RegisterDto>, RegisterValidator>();

        return services;
    }
}