using AgendamentoApi.Data;
using AgendamentoApi.Models;
using AgendamentoApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoApi.Repositories;

public class OwnershipRepository : IOwnershipRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<OwnershipRepository> _logger;

    public OwnershipRepository(AppDbContext context, ILogger<OwnershipRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Ownership?> GetOwnershipByIdAsync(Guid ownershipId)
    {
        try
        {
            return await _context.Ownerships.FirstOrDefaultAsync(o => o.Id == ownershipId);
        }
        catch (Exception)
        {
            _logger.LogError("Ownership id not found: {OwnershipId}", ownershipId);
            throw;
        }
    }

    public async Task<List<Ownership>> GetPropertyByOwnershipAsync(Guid ownerId)
    {
        try
        {
            var ownerships = await _context.Ownerships
                .Where(o => o.OwnerId == ownerId)
                .ToListAsync();
            
            return ownerships;
        }
        catch (Exception)
        {
            _logger.LogError("Error retrieving ownerships for owner with Id: {OwnerId}", ownerId);
            throw;
        }
    }

    public async Task<List<Ownership>> GetAllOwnershipAsync()
    {
        try
        {
            return await _context.Ownerships.ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all ownerships");
            throw;
        }
    }

    public async Task<Ownership> CreateOwnershipAsync(Ownership ownership)
    {
        try
        {
            _context.Ownerships.Add(ownership);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Ownership created: {OwnerId}", ownership.OwnerId);
            return ownership;
        }
        catch (Exception)
        {
            _logger.LogError("Error creating ownership: {OwnerId}", ownership.OwnerId);
            throw;
        }
    }

    public async Task<Ownership> UpdateOwnershipAsync(Guid ownershipId)
    {
        try
        {
            var existingOwnership = await _context.Ownerships.FindAsync(ownershipId);
            if (existingOwnership == null)
                throw new KeyNotFoundException($"Ownership with Id: {ownershipId} doesn't exist");

            _context.Entry(existingOwnership).CurrentValues.SetValues(ownershipId);
            await _context.SaveChangesAsync();
            return existingOwnership;
        }
        catch (Exception)
        {
            _logger.LogError("Error updating ownership: {OwnerId}", ownershipId);
            throw;
        }
    }

    public async Task<bool> DeleteOwnershipAsync(Guid ownershipId)
    {
        try
        {
            var ownership = await _context.Ownerships.FindAsync(ownershipId);
            if (ownership == null)
            {
                _logger.LogError("Ownership with Id: {OwnerId} doesn't exist", ownershipId);
                return false;
            }

            _context.Ownerships.Remove(ownership);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            _logger.LogError("Error deleting ownership: {OwnerId}", ownershipId);
            throw;
        }
    }
}