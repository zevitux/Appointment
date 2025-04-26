using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.Models;

public class Poperty
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [MaxLength(100)]
    public string Description { get; set; }

    public bool AcceptsPrePayment { get; set; }
    public User Owner { get; set; }
    public List<Appointment> Appointments { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}