using AgendamentoApi.Models;

namespace AgendamentoApi.Repositories.Interfaces;

public interface IOwnershipRepository
{
    Task<Ownership?> GetOwnershipByIdAsync(Guid ownershipId);
    Task<List<Ownership>> GetPropertyByOwnerAsync(Guid ownerId);
    Task<List<Ownership>> GetAllOwnershipAsync();
    Task<Ownership> CreateOwnershipAsync(Ownership ownership);
    Task<Ownership> UpdateOwnershipAsync(Guid ownershipId, Ownership updatedOwnership);
    Task<bool> DeleteOwnershipAsync(Guid ownershipId);
}