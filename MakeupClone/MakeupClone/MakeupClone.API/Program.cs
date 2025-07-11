using MakeupClone.API.Endpoints;
using MakeupClone.API.Middleware;
using MakeupClone.Application.Extensions;
using MakeupClone.Application.Interfaces;
using MakeupClone.Infrastructure.Data.MappingProfiles;
using MakeupClone.Infrastructure.Extensions;
using MakeupClone.Infrastructure.Secutiry;
using Microsoft.AspNetCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDatabase(builder.Configuration)
    .AddIdentityConfiguration()
    .AddAuthenticationConfiguration(builder.Configuration)
    .AddConfigurationSettings(builder.Configuration)
    .AddAuthorizationPolicies()
    .AddValidation()
    .AddRepositories()
    .AddServices()
    .AddPaymentServices()
    .AddDeliveryClients()
    .AddDeliveryServices();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IGoogleJsonWebSignatureWrapper, GoogleJsonWebSignatureWrapper>();

builder.Services.AddAutoMapper(typeof(ProductMappingProfile).Assembly);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapBrandEndpoints();
app.MapCategoryEndpoints();
app.MapAdminProductEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();

app.Run();