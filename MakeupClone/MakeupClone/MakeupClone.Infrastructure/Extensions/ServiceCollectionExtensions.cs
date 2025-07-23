using System.Text;
using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Delivery;
using MakeupClone.Infrastructure.Delivery.Clients.Implementations;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Payments;
using MakeupClone.Infrastructure.Repositories;
using MakeupClone.Infrastructure.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Stripe;

namespace MakeupClone.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MakeupCloneDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("MakeupCloneConnectionString")));

        return services;
    }

    public static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MakeupCloneDbContext>()
            .AddDefaultTokenProviders();

        return services;
    }

    public static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSettings = configuration.GetSection("Jwt");
        var googleSettings = configuration.GetSection("Google");

        var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] !);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };
        })
        .AddGoogle(options =>
        {
            options.ClientId = googleSettings["ClientId"] !;
            options.ClientSecret = googleSettings["ClientSecret"] !;
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationBuilder()
            .AddPolicy("AdminPolicy", policy =>
                policy.RequireRole("Admin"));

        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IBrandRepository, BrandRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }

    public static IServiceCollection AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AdminAccountSettings>(configuration.GetSection("AdminAccountSettings"));

        services.Configure<StripeOptions>(configuration.GetSection("Stripe"));

        services.Configure<NovaPoshtaOptions>(configuration.GetSection("NovaPoshta"));
        services.Configure<UkrPoshtaOptions>(configuration.GetSection("UkrPoshta"));
        services.Configure<MeestExpressOptions>(configuration.GetSection("MeestExpress"));

        return services;
    }

    public static IServiceCollection AddPaymentServices(this IServiceCollection services)
    {
        services.AddScoped<IPaymentService, StripePaymentService>();

        services.AddScoped(provider =>
        {
            var options = provider.GetRequiredService<IOptions<StripeOptions>>().Value;
            return new StripeClient(options.SecretKey);
        });

        services.AddScoped<PaymentIntentService>();

        return services;
    }

    public static IServiceCollection AddDeliveryClients(this IServiceCollection services)
    {
        services.AddScoped<INovaPoshtaClient, NovaPoshtaClient>();
        services.AddScoped<IUkrPoshtaClient, UkrPoshtaClient>();
        services.AddScoped<IMeestExpressClient, MeestExpressClient>();

        return services;
    }

    public static IServiceCollection AddDeliveryProviderFactory(this IServiceCollection services)
    {
        services.AddScoped<Func<DeliveryType, IDeliveryProvider>>(provider => deliveryType =>
        {
            return deliveryType switch
            {
                DeliveryType.NovaPoshta => provider.GetRequiredService<NovaPoshtaProvider>(),
                DeliveryType.UkrPoshta => provider.GetRequiredService<UkrPoshtaProvider>(),
                DeliveryType.MeestExpress => provider.GetRequiredService<MeestExpressProvider>(),
                _ => throw new NotSupportedException($"Delivery type '{deliveryType}' is not supported.")
            };
        });

        return services;
    }

    public static IServiceCollection AddDeliveryServices(this IServiceCollection services)
    {
        services.AddHttpClient<NovaPoshtaProvider>();
        services.AddHttpClient<UkrPoshtaProvider>();
        services.AddHttpClient<MeestExpressProvider>();

        services.AddScoped<NovaPoshtaProvider>();
        services.AddScoped<UkrPoshtaProvider>();
        services.AddScoped<MeestExpressProvider>();

        services.AddDeliveryProviderFactory();

        services.AddScoped<IDeliveryService, DeliveryService>();

        return services;
    }
}
