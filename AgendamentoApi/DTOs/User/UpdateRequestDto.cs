namespace AgendamentoApi.DTOs.User;

public class UpdateRequestDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public bool? IsBanned { get; set; }
}