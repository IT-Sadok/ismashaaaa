using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
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
        services.AddScoped<IValidationPipeline, ValidationPipeline>();

        return services;
    }
}