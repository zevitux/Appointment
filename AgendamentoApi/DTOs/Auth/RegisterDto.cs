using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.DTOs.Auth;

public class RegisterDto
{
    [MinLength(3), MaxLength(50)]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, MinLength(8)]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; }
}