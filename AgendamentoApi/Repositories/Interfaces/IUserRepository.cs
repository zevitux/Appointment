using AgendamentoApi.Models;

namespace AgendamentoApi.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<User>> GetAllUsersAsync();
    Task<User> CreateUserAsync(User user);
    Task<User> UpdateUserAsync(Guid userId);
    Task<bool>  DeleteUserAsync(Guid userId);
}