using AgendamentoApi.Models;

namespace AgendamentoApi.Repositories.Interfaces;

public interface IOwnershipRepository
{
    Task<Ownership?> GetOwnershipByIdAsync(Guid ownershipId);
    Task<List<Ownership>> GetPropertyByOwnershipAsync(Guid ownerId);
    Task<List<Ownership>> GetAllOwnershipAsync();
    Task<Ownership> CreateOwnershipAsync(Ownership ownership);
    Task<Ownership> UpdateOwnershipAsync(Guid ownershipId);
    Task<bool> DeleteOwnershipAsync(Guid ownershipId);
}