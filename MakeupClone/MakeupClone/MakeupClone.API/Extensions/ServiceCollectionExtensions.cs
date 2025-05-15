using MakeupClone.Domain.Entities;
using MakeupClone.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MakeupClone.API.Extensions;

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

        var key = Convert.FromBase64String(jwtSettings["Key"] !);

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
}