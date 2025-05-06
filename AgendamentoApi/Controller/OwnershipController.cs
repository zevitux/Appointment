using AgendamentoApi.DTOs.Ownership;
using AgendamentoApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AgendamentoApi.Controller;

[ApiController]
[Route("api/ownership")]
public class OwnershipController : ControllerBase
{
    private readonly IOwnershipService _ownershipService;
    private readonly ILogger<OwnershipController> _logger;

    public OwnershipController(IOwnershipService ownershipService, ILogger<OwnershipController> logger)
    {
        _ownershipService = ownershipService;
        _logger = logger;
    }

    [HttpGet("{ownershipId}")]
    public async Task<ActionResult<OwnershipResponseDto>> GetOwnershipById(Guid ownershipId)
    {
        try
        {
            var result = await _ownershipService.GetOwnershipByIdAsync(ownershipId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving ownership with Id: {ownershipId}");
            return NotFound(ex);
        }
    }

    [HttpGet("owner/{ownerId}")]
    public async Task<ActionResult<OwnershipResponseDto>> GetOwnershipByOwnerId(Guid ownerId)
    {
        try
        {
            var result = await _ownershipService.GetPropertiesByOwnerIdAsync(ownerId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving ownership with Id: {ownerId}");
            return NotFound(ex);
        }
    }

    [HttpGet]
    public async Task<ActionResult<List<OwnershipResponseDto>>> GetAllOwnershipAsync()
    {
        var result = await _ownershipService.GetAllOwnershipsAsync();
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OwnershipResponseDto>> CreateOwnershipAsync(OwnershipCreateRequestDto request)
    {
        try
        {
            var result = await _ownershipService.CreateOwnershipAsync(request);
            return CreatedAtAction(nameof(GetOwnershipById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating ownership");
            return BadRequest(ex);
        }
    }
    
    [HttpDelete("{ownershipId}")]
    public async Task<ActionResult> DeleteOwnership(Guid ownershipId)
    {
        try
        {
            var deleted = await _ownershipService.DeleteOwnershipAsync(ownershipId);
            if (deleted)
                return NoContent();
            return NotFound($"Ownership with Id: {ownershipId} not found or could not be deleted");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting ownership with Id: {ownershipId}");
            return BadRequest(ex.Message);
        }
    }
}