using AgendamentoApi.Models;

namespace AgendamentoApi.Repositories.Interfaces;

public interface ITransactionRepository
{
    Task<Transaction?> GetTransactionById(Guid transactionId);
    Task<List<Transaction>> GetAllTransactionsByUserId(Guid userId);
    Task<Transaction> CreateTransactionAsync(Guid appointmentId, Guid userId, decimal amount, string paymentMethod);
    Task<bool> MarkTransactionAsPaidAsync(Guid transactionId);
    Task<bool> MarkTransactionAsCancelledAsync (Guid transactionId);
    Task<bool> MarkTransactionAsRefundedAsync(Guid transactionId);
}