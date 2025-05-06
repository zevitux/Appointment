using AgendamentoApi.DTOs.Ownership;

namespace AgendamentoApi.Services.Interfaces;

public interface IOwnershipService
{
    Task<OwnershipResponseDto> GetOwnershipByIdAsync(Guid ownershipId);
    Task<List<OwnershipResponseDto>> GetPropertiesByOwnerIdAsync(Guid ownerId);
    Task<List<OwnershipResponseDto>> GetAllOwnershipsAsync();
    Task<OwnershipResponseDto> CreateOwnershipAsync(OwnershipCreateRequestDto request);
    Task<OwnershipResponseDto> UpdateOwnershipAsync(Guid ownershipId, OwnershipUpdateRequestDto request);
    Task<bool> DeleteOwnershipAsync(Guid ownershipId);
}