using System.Security.Claims;
using System.Security.Cryptography;
using AgendamentoApi.Data;
using AgendamentoApi.DTOs.Auth;
using AgendamentoApi.Helpers;
using AgendamentoApi.Models;
using AgendamentoApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoApi.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly AppDbContext _context;
    private readonly ILogger<AuthService> _logger;
    private readonly JwtHelper _jwtHelper;

    public AuthService(AppDbContext context, ILogger<AuthService> logger, JwtHelper jwtHelper)
    {
        _context = context;
        _logger = logger;
        _jwtHelper = jwtHelper;
    }
    
    public async Task<TokenResponseDto?> LoginAsync(LoginDto request)
    {
        _logger.LogInformation($"Login attempt for user with Email: {request.Email}");
        var normalizedEmail = request.Email.Trim().ToLower();
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == normalizedEmail);

        if (user == null)
        {
            _logger.LogWarning($"User with Email: {normalizedEmail} not found");
            return null;
        }

        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, request.Password) ==
            PasswordVerificationResult.Failed)
        {
            _logger.LogWarning("Login attempt failed for {Email}", request.Email);
            return null;
        }

        return await CreateTokenResponse(user);
    }

    public async Task<User?> RegisterAsync(RegisterDto request)
    {
        _logger.LogInformation($"Register attempt for user with Email: {request.Email}");
        var normalizedEmail = request.Email.Trim().ToLower();
        if (await _context.Users.AnyAsync(x => x.Email == normalizedEmail))
        {
            _logger.LogWarning($"User with Email: {normalizedEmail} not found");
            return null;
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length < 3)
        {
            _logger.LogWarning($"User with Name: {request.Name} is invalid");
            return null;
        }

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 8)
        {
            _logger.LogWarning("Password must be greater than 8 characters");
            return null;
        }

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Email = request.Email,
            Role = "User", // Default role
            IsBanned = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };
        
        user.PasswordHash = new PasswordHasher<User>().HashPassword(user, request.Password);

        try
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"User with Email: {normalizedEmail} created");
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user");
            return null;
        }
    }

    public async Task<TokenResponseDto?> RefreshTokenAsync(RefreshTokenRequestDto request)
    {
        _logger.LogInformation("Refresh token attempt for user with Id: {UserId}", request.UserId);
        var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

        if (user == null)
        {
            _logger.LogWarning($"User with Id: {request.UserId} not found");
            return null;
        }

        return await CreateTokenResponse(user);
    }

    public async Task<bool> LogoutAsync(Guid userId)
    {
        _logger.LogInformation("Logout attempt for the user with ID: {UserId}", userId);
        var user = await _context.Users.FindAsync(userId);

        if (user == null)
        {
            _logger.LogWarning($"User with Id: {userId} not found");
            return false;
        }
        
        user.RefreshToken = null;
        user.RefreshTokenExpiryTime = DateTime.MinValue;
        
        await _context.SaveChangesAsync(); 
        
        _logger.LogInformation($"User with Id: {userId} logged out");
        return true;
    }

    private async Task<TokenResponseDto?> CreateTokenResponse(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };
        
        var accessToken = _jwtHelper.CreateToken(claims);
        var refreshToken = await GenerateAndSaveRefreshTokenAsync(user);

        return new TokenResponseDto()
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken!
        };
    }
    private async Task<User?> ValidateRefreshTokenAsync(Guid userId, string refreshToken)
    {
        _logger.LogInformation("Validating refresh token for user with Id: {userId}", userId);
        
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            _logger.LogWarning($"User with Id: {userId} not found");
            return null;
        }
        
        _logger.LogInformation("Refresh token validated for user with Id: {userId}", userId);
        
        return user;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber)
            .Replace("/", "_")
            .Replace("+", "-")
            .Replace("=","");
    }

    private async Task<string?> GenerateAndSaveRefreshTokenAsync(User? user)
    {
        if(user == null) return null;
        
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        
        await _context.SaveChangesAsync(); 
        
        _logger.LogInformation("Refresh token generated and saved for user with Email: {Email}", user.Email);
        return refreshToken;
    }
}