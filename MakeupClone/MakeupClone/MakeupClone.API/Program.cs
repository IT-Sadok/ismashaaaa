using MakeupClone.API.Endpoints;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Interfaces;
using MakeupClone.Infrastructure.Data.MappingProfiles;
using MakeupClone.Infrastructure.Extensions;
using MakeupClone.Infrastructure.Secutiry;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddDatabase(builder.Configuration)
    .AddIdentityConfiguration()
    .AddAuthenticationConfiguration(builder.Configuration)
    .AddConfigurationSettings(builder.Configuration)
    .AddAuthorizationPolicies()
    .AddCustomValidators()
    .AddRepositories()
    .AddServices();

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IGoogleJsonWebSignatureWrapper, GoogleJsonWebSignatureWrapper>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapAuthEndpoints();
app.MapBrandEndpoints();
app.MapCategoryEndpoints();
app.MapAdminProductEndpoints();
app.MapProductEndpoints();

app.Run();