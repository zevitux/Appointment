using AgendamentoApi.Data;
using AgendamentoApi.Models;
using AgendamentoApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoApi.Repositories;

public class TransactionRepository : ITransactionRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<TransactionRepository> _logger;

    public TransactionRepository(AppDbContext context, ILogger<TransactionRepository> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    public async Task<Transaction?> GetTransactionById(Guid transactionId)
    {
        try
        {
            return await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying find the transaction with ID: {TransactionId}", transactionId);
            throw;
        }
    }

    public async Task<List<Transaction>> GetAllTransactionsByUserId(Guid userId)
    {
        try
        {
            return await _context.Transactions
                .Where(x => x.UserId == userId)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error trying find the transactions with ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<Transaction> CreateTransactionAsync(Guid appointmentId, Guid userId, decimal amount, string paymentMethod)
    {
        try
        {
            var transaction = new Transaction
            {
                AppointmentId = appointmentId,
                UserId = userId,
                Amount = amount,
                PaymentDate = DateTime.UtcNow,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Pending",
                IsCancelled = false,
                IsRefunded = false
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating the transaction for user with ID: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> MarkTransactionAsPaidAsync(Guid transactionId)
    {
        try
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
            transaction!.IsPaid = true;
            transaction.PaymentStatus = "Paid";

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Error making the transaction as it is paid");
            throw;
        }
    }


    public async Task<bool> MarkTransactionAsCancelledAsync(Guid transactionId)
    {
        try
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
            transaction!.IsCancelled = true;
            transaction.PaymentStatus = "Cancelled";

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking the transaction as it is cancelled");
            throw;
        }
    }

    public async Task<bool> MarkTransactionAsRefundedAsync(Guid transactionId)
    {
        try
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(x => x.Id == transactionId);
            transaction!.IsRefunded = true;
            transaction.PaymentStatus = "Refunded";

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking the transaction as it is refunded");
            throw;
        }
    }
}