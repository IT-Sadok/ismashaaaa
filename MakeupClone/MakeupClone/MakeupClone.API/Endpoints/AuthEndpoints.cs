using MakeupClone.Application.DTOs.Auth;
using MakeupClone.Application.Interfaces;

namespace MakeupClone.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        app.MapPost("/api/auth/register", RegisterAsync);
        app.MapPost("/api/auth/login", LoginAsync);
        app.MapPost("/api/auth/google-login", GoogleLoginAsync);
    }

    private static async Task<IResult> RegisterAsync(IAuthService authService, RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var registerResult = await authService.RegisterAsync(registerDto, cancellationToken);
        return registerResult.Success ? Results.Ok(registerResult) : Results.BadRequest(registerResult);
    }

    private static async Task<IResult> LoginAsync(IAuthService authService, LoginDto loginDto, CancellationToken cancellationToken)
    {
        var loginResult = await authService.LoginAsync(loginDto, cancellationToken);
        return loginResult.Success ? Results.Ok(loginResult) : Results.Unauthorized();
    }

    private static async Task<IResult> GoogleLoginAsync(IAuthService authService, GoogleLoginDto googleLoginDto, CancellationToken cancellationToken)
    {
        var googleLoginResult = await authService.GoogleLoginAsync(googleLoginDto, cancellationToken);
        return googleLoginResult.Success ? Results.Ok(googleLoginResult) : Results.Unauthorized();
    }
}