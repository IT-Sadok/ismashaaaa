using FluentValidation;
using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Application.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace MakeupClone.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IAdminProductService, AdminProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IBrandService, BrandService>();
        services.AddScoped<IOrderService, OrderService>();

        return services;
    }

    public static IServiceCollection AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

        return services;
    }
}