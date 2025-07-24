using MakeupClone.API.Endpoints;
using MakeupClone.API.Middleware;
using MakeupClone.Application.Extensions;
using MakeupClone.Application.Interfaces;
using MakeupClone.Infrastructure.Data.MappingProfiles;
using MakeupClone.Infrastructure.Extensions;
using MakeupClone.Infrastructure.Secutiry;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

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

app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    options.RoutePrefix = string.Empty;
});

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