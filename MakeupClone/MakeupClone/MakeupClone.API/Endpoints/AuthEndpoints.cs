﻿using MakeupClone.API.Constants;
using MakeupClone.API.Filters;
using MakeupClone.Application.DTOs.Auth;
using MakeupClone.Application.Interfaces;

namespace MakeupClone.API.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication application)
    {
        var group = application.MapGroup(ApiRoutes.Auth.Base);

        group.MapPost(ApiRoutes.Auth.Register, RegisterAsync)
            .AddEndpointFilter<ValidationFilter<RegisterDto>>();
        group.MapPost(ApiRoutes.Auth.Login, LoginAsync)
            .AddEndpointFilter<ValidationFilter<LoginDto>>();
        group.MapPost(ApiRoutes.Auth.GoogleLogin, GoogleLoginAsync);
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