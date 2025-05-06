using AgendamentoApi.DTOs.User;
using AgendamentoApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoApi.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserResponseDto>> GetUserById(Guid userId)
    {
        try
        {
            var user = await _userService.GetUserByIdAsync(userId);
            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user with Id: {Id}", userId);
            throw;
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<UserResponseDto>>> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all users");
            throw;
        }
    }

    [HttpPost]
    public async Task<ActionResult<UserResponseDto>> CreateUser(CreateUserRequestDto request)
    {
        try
        {
            var createdUser = await _userService.CreateUserAsync(request);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with Email: {Email}", request.Email);
            throw;
        }
    }
    
    [HttpPut("{userId}")]
    public async Task<ActionResult<UserResponseDto>> UpdateUser(Guid userId, UpdateRequestDto request)
    {
        try
        {
            var updatedUser = await _userService.UpdateUserAsync(userId, request);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with Id: {Id}", userId);
            throw;
        }
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<UserResponseDto>> DeleteUser(Guid userId)
    {
        try
        {
            var deletedUser = await _userService.DeleteUserAsync(userId);
            if (!deletedUser)
                return NotFound("User not found!");

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting user with Id: {Id}", userId);
            throw;
        }
    }
}