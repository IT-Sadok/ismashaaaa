using MakeupClone.Application.DTOs;

namespace MakeupClone.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> Register(RegisterDto registerDto, CancellationToken cancellationToken);

    Task<AuthResultDto> Login(LoginDto loginDto, CancellationToken cancellationToken);

    Task<AuthResultDto> GoogleLogin(GoogleLoginDto gooleLoginDto, CancellationToken cancellationToken);
}