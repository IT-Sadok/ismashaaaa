using MakeupClone.Application.DTOs;
using MakeupClone.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MakeupClone.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        var registerResult = await _authService.RegisterAsync(registerDto, HttpContext.RequestAborted);
        return registerResult.Success ? Ok(registerResult) : BadRequest(registerResult);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var loginResult = await _authService.LoginAsync(loginDto, HttpContext.RequestAborted);
        return loginResult.Success ? Ok(loginResult) : Unauthorized(loginResult);
    }

    [HttpPost("google-login")]
    public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginDto googleLoginDto)
    {
        var googleLoginResult = await _authService.GoogleLoginAsync(googleLoginDto, HttpContext.RequestAborted);
        return googleLoginResult.Success ? Ok(googleLoginResult) : Unauthorized(googleLoginResult);
    }
}