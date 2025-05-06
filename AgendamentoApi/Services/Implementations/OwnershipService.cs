using AgendamentoApi.DTOs.Ownership;
using AgendamentoApi.Models;
using AgendamentoApi.Repositories.Interfaces;
using AgendamentoApi.Services.Interfaces;

namespace AgendamentoApi.Services.Implementations;

public class OwnershipService : IOwnershipService
{
    private readonly IOwnershipRepository _ownershipRepository;
    private readonly ILogger<OwnershipService> _logger;

    public OwnershipService(IOwnershipRepository ownershipRepository, ILogger<OwnershipService> logger)
    {
        _ownershipRepository = ownershipRepository;
        _logger = logger;
    }
    
    public async Task<OwnershipResponseDto> GetOwnershipByIdAsync(Guid ownershipId)
    {
        try
        {
            var ownership = await _ownershipRepository.GetOwnershipByIdAsync(ownershipId);

            if (ownership == null)
            {
                _logger.LogError($"Ownership with Id: {ownershipId} not found");
                throw new Exception("Ownership not found or don't exist");
            }

            _logger.LogInformation($"Ownership with Id: {ownershipId} has been retrieved");
            return MapToResponseDto(ownership);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving ownership with Id: {ownershipId}");
            throw;
        }
    }

    public async Task<List<OwnershipResponseDto>> GetPropertiesByOwnerIdAsync(Guid ownerId)
    {
        try
        {
            var ownership = await _ownershipRepository.GetPropertyByOwnerAsync(ownerId);

            if (ownership == null)
            {
                _logger.LogError($"Owner with Id: {ownerId} not found");
                throw new Exception("Owner not found or don't exist");
            }

            _logger.LogInformation($"Owner with Id: {ownerId} has been retrieved");
            return ownership.Select(MapToResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving ownership with Id: {ownerId}");
            throw;
        }
    }

    public async Task<List<OwnershipResponseDto>> GetAllOwnershipsAsync()
    {
        try
        {
            var ownership = await _ownershipRepository.GetAllOwnershipAsync();

            _logger.LogInformation("Ownership retrieved: {Count}", ownership.Count());

            return ownership.Select(MapToResponseDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving ownership");
            throw;
        }
    }

    public async Task<OwnershipResponseDto> CreateOwnershipAsync(OwnershipCreateRequestDto request)
    {
        try
        {
            var ownership = new Ownership
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                AcceptsPrePayment = request.AcceptsPrePayment,
                OwnerId = request.OwnerId,
                Appointments = new List<Appointment>(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            var createdOwnership = await _ownershipRepository.CreateOwnershipAsync(ownership);

            _logger.LogInformation($"Ownership created: {createdOwnership.Id}");
            return MapToResponseDto(createdOwnership);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating ownership");
            throw;
        }
    }

    public async Task<OwnershipResponseDto> UpdateOwnershipAsync(Guid ownershipId, OwnershipUpdateRequestDto request)
    {
        try
        {
            var ownership = await _ownershipRepository.GetOwnershipByIdAsync(ownershipId);
            if (ownership == null)
            {
                _logger.LogError($"Ownership with Id: {ownershipId} not found");
                throw new Exception("Ownership not found or don't exist");
            }

            ownership.Name = request.Name;
            ownership.Description = request.Description;
            ownership.AcceptsPrePayment = request.AcceptsPrePayment;
            ownership.OwnerId = request.OwnerId;
            ownership.UpdatedAt = DateTime.UtcNow;

            var updatedOwnership = await _ownershipRepository.UpdateOwnershipAsync(ownershipId, ownership);

            _logger.LogInformation($"Ownership updated: {ownership.Id}");
            return MapToResponseDto(updatedOwnership);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating ownership");
            throw;
        }
    }

    public async Task<bool> DeleteOwnershipAsync(Guid ownershipId)
    {
        try
        {
            var result = await _ownershipRepository.DeleteOwnershipAsync(ownershipId);

            if (result)
                _logger.LogInformation($"Ownership deleted: {ownershipId}");

            else
                _logger.LogError($"Ownership could not be deleted: {ownershipId}");

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting ownership");
            throw;
        }
    }

    private OwnershipResponseDto MapToResponseDto(Ownership ownership)
    {
        return new OwnershipResponseDto
        {
            Id = ownership.Id,
            Name = ownership.Name,
            Description = ownership.Description,
            AcceptsPrePayment = ownership.AcceptsPrePayment,
            OwnerId = ownership.OwnerId,
            CreatedAt = ownership.CreatedAt,
            UpdatedAt = ownership.UpdatedAt
        };
    }
}