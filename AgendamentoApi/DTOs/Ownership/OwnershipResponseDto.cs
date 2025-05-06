using AgendamentoApi.Models;

namespace AgendamentoApi.DTOs.Ownership;

public class OwnershipResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool AcceptsPrePayment { get; set; }
    public Guid OwnerId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}