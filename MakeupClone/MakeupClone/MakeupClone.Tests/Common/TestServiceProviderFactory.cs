using FluentValidation;
using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Services;
using MakeupClone.Application.Validators;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.MappingProfiles;
using MakeupClone.Infrastructure.Delivery;
using MakeupClone.Infrastructure.Delivery.Clients.Implementations;
using MakeupClone.Infrastructure.Delivery.Clients.Interfaces;
using MakeupClone.Infrastructure.Payments;
using MakeupClone.Infrastructure.Repositories;
using MakeupClone.Infrastructure.Secutiry;
using MakeupClone.Infrastructure.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Stripe;

namespace MakeupClone.Tests.Common;

public static class TestServiceProviderFactory
{
    public static ServiceProvider Create()
    {
        var services = new ServiceCollection();

        services.AddLogging();

        AddDatabase(services);
        AddIdentity(services);
        AddAutoMapper(services);
        AddValidators(services);
        AddConfiguration(services);
        AddConfigurationSettings(services);
        AddJwtTokenGenerator(services);
        AddGoogleJsonWebSignatureMock(services);
        AddRepositories(services);
        AddServices(services);
        AddPaymentServices(services);
        AddDeliveryClients(services);
        AddDeliveryServices(services);

        return services.BuildServiceProvider();
    }

    private static void AddDatabase(IServiceCollection services)
    {
        services.AddDbContext<MakeupCloneDbContext>(options =>
            options.UseNpgsql(TestConfiguration.ConnectionString));

        services.AddLogging();
    }

    private static void AddIdentity(IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireLowercase = false;
        })
            .AddEntityFrameworkStores<MakeupCloneDbContext>()
            .AddDefaultTokenProviders();
    }

    private static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<RegisterValidator>();
    }

    private static void AddConfiguration(IServiceCollection services)
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Jwt:Key", "super_secret_test_key_1234567890123456" },
            { "Jwt:Issuer", "TestIssuer" },
            { "Jwt:Audience", "TestAudience" },
            { "Jwt:TokenExpirationMinutes", "60" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        services.AddSingleton(configuration);
    }

    private static void AddConfigurationSettings(IServiceCollection services)
    {
        var configuration = TestConfiguration.Configuration;

        services.AddSingleton<IConfiguration>(configuration);

        services.Configure<StripeOptions>(configuration.GetSection("Stripe"));
        services.Configure<NovaPoshtaOptions>(configuration.GetSection("NovaPoshta"));
        services.Configure<UkrPoshtaOptions>(configuration.GetSection("UkrPoshta"));
        services.Configure<MeestExpressOptions>(configuration.GetSection("MeestExpress"));
    }

    private static void AddJwtTokenGenerator(IServiceCollection services)
    {
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
    }

    private static void AddGoogleJsonWebSignatureMock(IServiceCollection services)
    {
        var googleWrapperMock = new Mock<IGoogleJsonWebSignatureWrapper>();
        services.AddSingleton(googleWrapperMock);
        services.AddSingleton(
            provider => provider.GetRequiredService<Mock<IGoogleJsonWebSignatureWrapper>>().Object);
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddScoped<IAdminProductService, AdminProductService>();
        services.AddScoped<IOrderService, OrderService>();
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

        services.AddScoped<IDeliveryProvider, NovaPoshtaProvider>();
        services.AddScoped<IDeliveryProvider, UkrPoshtaProvider>();
        services.AddScoped<IDeliveryProvider, MeestExpressProvider>();

        services.AddDeliveryProviderFactory();

        services.AddScoped<IDeliveryService, DeliveryService>();

        return services;
    }
}