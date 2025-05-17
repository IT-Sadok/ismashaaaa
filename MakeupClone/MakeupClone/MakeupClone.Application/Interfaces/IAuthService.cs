using MakeupClone.Application.DTOs;

namespace MakeupClone.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResultDto> RegisterAsync(RegisterDto registerDto, CancellationToken cancellationToken = default);

    Task<AuthResultDto> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);

    Task<AuthResultDto> GoogleLoginAsync(GoogleLoginDto gooleLoginDto, CancellationToken cancellationToken = default);
}