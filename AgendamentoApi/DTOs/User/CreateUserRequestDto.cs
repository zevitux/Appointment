using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.DTOs.User;

public class CreateUserRequestDto
{
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [Required, MinLength(8)]
    public string Password { get; set; }
    [Required]
    public string Role { get; set; } //Admin, User, Seller
}