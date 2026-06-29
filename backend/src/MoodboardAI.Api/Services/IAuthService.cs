using MoodboardAI.Api.DTOs.Auth;

namespace MoodboardAI.Api.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto user);
    Task<AuthResponseDto> LoginAsync(LoginRequestDto user);
}