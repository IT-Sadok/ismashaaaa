using MakeupClone.Application.DTOs;

namespace MakeupClone.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> Register(RegisterDto registerDto);

        Task<AuthResultDto> Login(LoginDto loginDto);

        Task<AuthResultDto> GoogleLogin(GoogleLoginDto gooleLoginDto);
    }
}
