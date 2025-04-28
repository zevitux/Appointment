using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.DTOs.Auth;

public class LoginDto
{
    [Required, EmailAddress(ErrorMessage = "Invalid credentials")]
    public string Email { get; set; }
    [Required(ErrorMessage = "Invalid credentials")]
    public string Password { get; set; }
}