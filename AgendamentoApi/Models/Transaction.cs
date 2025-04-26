using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.Models;

public class Transaction
{
    [Key]
    public Guid Id { get; set; }
    public Guid AppointmentId { get; set; }
    public Appointment Appointment { get ; set; } = new();
    public string PaymentMethod { get; set; } //Pix or PayPal
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string PaymentStatus { get; set; } //Pending, Paid and Failed
}