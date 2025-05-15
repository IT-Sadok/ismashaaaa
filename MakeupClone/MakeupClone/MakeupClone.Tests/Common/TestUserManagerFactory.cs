using AutoMapper;
using FluentValidation;
using MakeupClone.Application.DTOs;
using MakeupClone.Application.Interfaces;
using MakeupClone.Application.Validators;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Interfaces;
using MakeupClone.Infrastructure.Data;
using MakeupClone.Infrastructure.Data.MappingProfiles;
using MakeupClone.Infrastructure.Secutiry;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

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
        AddJwtTokenGenerator(services);
        AddGoogleJsonWebSignatureMock(services);

        return services.BuildServiceProvider();
    }

    private static void AddDatabase(IServiceCollection services)
    {
        services.AddDbContext<MakeupCloneDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5433;Database=MakeupCloneTestDb;Username=userForTests;Password=hnYN7~8/1@_a"));

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
        var mapperConfig = new MapperConfiguration(configuration =>
        {
            configuration.AddProfile<MappingProfile>();
        });

        services.AddSingleton(mapperConfig.CreateMapper());
    }

    private static void AddValidators(IServiceCollection services)
    {
        services.AddTransient<IValidator<RegisterDto>, RegisterValidator>();
        services.AddTransient<IValidator<LoginDto>, LoginValidator>();
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
}