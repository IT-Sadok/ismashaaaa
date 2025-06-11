using MakeupClone.Application.DTOs.Auth;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace MakeupClone.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IGoogleJsonWebSignatureWrapper _googleSignature;

    public AuthService(
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IGoogleJsonWebSignatureWrapper googleSignature)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _googleSignature = googleSignature;
    }

    public async Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default)
    {
        var user = new User
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            UserName = registerDto.Email,
            BirthDate = registerDto.BirthDate,
            PhoneNumber = registerDto.PhoneNumber,
            Email = registerDto.Email,
            ReceiveNotifications = registerDto.ReceiveNotifications,
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (!result.Succeeded)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = result.Errors.Select(error => error.Description)
            };
        }

        await _userManager.AddToRoleAsync(user, Roles.User);

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResultDto { Success = true, Token = token };
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return new AuthResultDto { Success = false, Errors = new[] { "Invalid email or password." } };

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResultDto() { Success = true, Token = token };
    }

    public async Task<AuthResultDto> GoogleLoginAsync(GoogleLoginDto gooleLoginDto, CancellationToken cancellationToken = default)
    {
        var payload = await _googleSignature.Validate(gooleLoginDto.IdToken);
        var user = await _userManager.FindByEmailAsync(payload.Email);

        if (user == null)
        {
            user = new User
            {
                Email = payload.Email,
                UserName = payload.Email,
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
            };

            var result = await _userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                return new AuthResultDto
                {
                    Success = false,
                    Errors = result.Errors.Select(error => error.Description)
                };
            }
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResultDto() { Success = true, Token = token };
    }
}