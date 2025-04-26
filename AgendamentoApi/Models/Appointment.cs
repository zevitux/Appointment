using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.Models;

public class Appointment
{
    [Key]
    public Guid Id { get; set; }
    public Guid OwnershipId { get; set; }
    public Ownership Ownership { get; set; } = new();
    public Guid UserId { get; set; }
    public User User { get; set; } = new();
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } //Pending, Confirmed and Canceled
    public string PaymentStatus { get; set; } //Pending, Paid and NotRequired
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}