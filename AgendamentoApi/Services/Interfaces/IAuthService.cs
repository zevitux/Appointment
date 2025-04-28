using AgendamentoApi.DTOs.Auth;
using AgendamentoApi.Models;

namespace AgendamentoApi.Services.Interfaces;

public interface IAuthService
{
    Task<TokenResponseDto?> LoginAsync(LoginDto request);
    Task<User?> RegisterAsync(RegisterDto request);
    Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request);
    Task<bool> LogoutAsync(Guid userId);
}