using AgendamentoApi.DTOs.Auth;
using AgendamentoApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        try
        {
            var user = await authService.RegisterAsync(dto);

            if (user == null)
            {
                logger.LogWarning($"Error registering user {dto.Email}");
                return BadRequest("Error registering user");
            }

            logger.LogInformation("User {Email} registered successfully.", dto.Email);
            return Ok(user);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error registering user");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        try
        {
            var tokenResponse = await authService.LoginAsync(dto);

            if (tokenResponse == null)
            {
                logger.LogWarning($"Error logging in {dto.Email}");
                return Unauthorized("Invalid credentials");
            }

            return Ok(tokenResponse);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error logging in");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequestDto dto)
    {
        try
        {
            var result = await authService.RefreshTokenAsync(dto);

            if (result == null)
            {
                logger.LogWarning($"Error refreshing token {dto.RefreshToken}");
                return Unauthorized("Invalid RefreshToken credentials");
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Error refreshing token");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(LogoutRequestDto dto)
    {
        try
        {
            var result = await authService.LogoutAsync(dto.UserId);
            if (!result)
            {
                logger.LogWarning($"Error logging out user {dto.UserId}");
                return BadRequest("Error logging out");
            }

            logger.LogInformation("User {UserId} logged out successfully", dto.UserId);
            return Ok("Logout successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error logging out");
            return StatusCode(500, "Internal server error");
        }
    }
}