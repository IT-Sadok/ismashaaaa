using FluentValidation;
using MakeupClone.Application.DTOs;
using MakeupClone.Application.Interfaces;
using MakeupClone.Domain.Entities;
using MakeupClone.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MakeupClone.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IValidator<RegisterDto> _registrationValidator;
    private readonly IValidator<LoginDto> _loginValidator;
    private readonly IGoogleJsonWebSignatureWrapper _googleSignature;

    public AuthService(
        UserManager<User> userManager,
        IJwtTokenGenerator jwtTokenGenerator,
        IValidator<RegisterDto> registrationValidator,
        IValidator<LoginDto> loginValidator,
        IGoogleJsonWebSignatureWrapper googleSignature)
    {
        _userManager = userManager;
        _jwtTokenGenerator = jwtTokenGenerator;
        _registrationValidator = registrationValidator;
        _loginValidator = loginValidator;
        _googleSignature = googleSignature;
    }

    public async Task<AuthResultDto> Register(RegisterDto registerDto, CancellationToken cancellationToken)
    {
        var validationResponse = await ValidateRequest(registerDto, _registrationValidator, cancellationToken);
        if (validationResponse != null) return validationResponse;

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

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResultDto { Success = true, Token = token };
    }

    public async Task<AuthResultDto> Login(LoginDto loginDto, CancellationToken cancellationToken)
    {
        var validationResponse = await ValidateRequest(loginDto, _loginValidator, cancellationToken);
        if (validationResponse != null) return validationResponse;

        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, loginDto.Password))
            return new AuthResultDto { Success = false, Errors = new[] { "Invalid email or password." } };

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResultDto() { Success = true, Token = token };
    }

    public async Task<AuthResultDto> GoogleLogin(GoogleLoginDto gooleLoginDto, CancellationToken cancellationToken)
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
                return new AuthResultDto
                {
                    Success = false,
                    Errors = result.Errors.Select(error => error.Description)
                };
        }

        var token = _jwtTokenGenerator.GenerateToken(user.Id, user.Email);
        return new AuthResultDto() { Success = true, Token = token };
    }

    private static async Task<AuthResultDto> ValidateRequest<T>(T request, IValidator<T> validator, CancellationToken cancellationToken)
    {
        var validationResponse = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResponse.IsValid)
        {
            return new AuthResultDto
            {
                Success = false,
                Errors = validationResponse.Errors.Select(error => error.ErrorMessage)
            };
        }

        return null;
    }
}