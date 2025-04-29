using AgendamentoApi.Data;
using AgendamentoApi.Models;
using AgendamentoApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoApi.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<UserRepository> _logger;

    public UserRepository(AppDbContext context, ILogger<UserRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<User?> GetUserByIdAsync(Guid userId)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        catch (Exception)
        {
            _logger.LogError("Error searching user with ID: {userId}", userId);
            throw;
        }
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        try
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        catch (Exception)
        {
            _logger.LogError("Error searching user with email: {email}", email);
            throw;
        }
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        try
        {
            return await _context.Users.ToListAsync();
        }
        catch (Exception)
        {
            _logger.LogError("Error searching all users!");
            throw;
        }
    }

    public async Task<User> CreateUserAsync(User user)
    {
        try
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
               _logger.LogWarning("User already exists: {Email}", user.Email);
               return null!;
            }
            
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
        catch (Exception)
        {
            _logger.LogError("Error creating user: {user}", user);
            throw;
        }
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        try
        {
            var existingUser = await _context.Users.FindAsync(user);
            if (existingUser == null)
                throw new KeyNotFoundException("User not found!");

            await _context.SaveChangesAsync();
            return existingUser;
        }
        catch (Exception)
        {
            _logger.LogError("Error updating user: {UserId}", user);
            throw;
        }
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                _logger.LogError("User with ID: {UserId} not found!", userId);
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            _logger.LogError("Error deleting user: {UserId}", userId);
            throw;
        }
    }
}