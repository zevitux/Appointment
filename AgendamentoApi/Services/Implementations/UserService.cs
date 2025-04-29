using AgendamentoApi.DTOs.User;
using AgendamentoApi.Models;
using AgendamentoApi.Repositories.Interfaces;
using AgendamentoApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace AgendamentoApi.Services.Implementations;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly ILogger<UserService> _logger;

    public UserService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher,
        ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }
    
    public async Task<UserResponseDto> GetUserByIdAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("User with Id: {UserId} not found or doesn't exist!", userId);
                throw new Exception("User not found or doesn't exist!");
            }

            _logger.LogInformation("User with Id: {UserId} has been retrieved", userId);
            return MapToResponseDto(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching user with ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<List<UserResponseDto>> GetAllUsersAsync()
    {
        try
        {
            var users = await _userRepository.GetAllUsersAsync();

            _logger.LogInformation("Users retrieved: {Count}", users.Count());

            return users.Select(MapToResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching user list");
            throw;
        }
    }

    public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto request)
    {
        try
        {
            var normalizedEmail = request.Email.Trim().ToLower();
            var existingUser = await _userRepository.GetUserByEmailAsync(normalizedEmail);

            if (existingUser != null)
            {
                _logger.LogWarning("User with Email: {Email} already exists!", request.Email);
                throw new Exception($"User with Email: {request.Email} already exists!");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Email = normalizedEmail,
                Role = request.Role,
                IsBanned = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            var createdUser = await _userRepository.CreateUserAsync(user);

            _logger.LogInformation("User {Email} created successfully!", request.Email);
            return MapToResponseDto(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email {Email}", request.Email);
            throw;
        }
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateRequestDto request)
    {
        try
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User with Id: {UserId} not found or doesn't exist!", userId);
                throw new Exception("User not found or doesn't exist!");
            }

            if (!string.IsNullOrEmpty(request.Name))
                user.Name = request.Name!.Trim();

            if (!string.IsNullOrEmpty(request.Email))
                user.Email = request.Email!.Trim().ToLower();

            if (!string.IsNullOrEmpty(request.Password))
                user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            user.Name = request.Name!.Trim();
            user.Email = request.Email!.Trim();
            user.Role = request.Role!.Trim();
            user.IsBanned = request.IsBanned ?? user.IsBanned;

            await _userRepository.UpdateUserAsync(user);

            return MapToResponseDto(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with email {Email}", request.Email);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        try
        {
            var result = await _userRepository.DeleteUserAsync(userId);
            
            if (result)
                _logger.LogInformation("User with Id: {UserId} has been deleted", userId);
            
            else
                _logger.LogInformation("User with Id {UserId} not found or doesn't exist!", userId);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with id {UserId}", userId);
            throw;
        }
    }

    private UserResponseDto MapToResponseDto(User user)
    {
        return new UserResponseDto
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            IsBanned = user.IsBanned,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
        };
    }
}