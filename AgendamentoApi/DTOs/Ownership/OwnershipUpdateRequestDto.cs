namespace AgendamentoApi.DTOs.Ownership;

public class OwnershipUpdateRequestDto
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool AcceptsPrePayment { get; set; }
    public Guid OwnerId { get; set; }
}