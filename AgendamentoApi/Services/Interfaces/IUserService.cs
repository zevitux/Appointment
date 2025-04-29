using AgendamentoApi.DTOs.User;

namespace AgendamentoApi.Services.Interfaces;

public interface IUserService
{
    Task<UserResponseDto> GetUserByIdAsync(Guid userId);
    Task<List<UserResponseDto>> GetAllUsersAsync();
    Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request);
    Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateRequestDto request);
    Task<bool> DeleteUserAsync(Guid userId);
}