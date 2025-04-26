using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.Models;

public class User
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required, EmailAddress]
    public string Email { get; set; }
    [MinLength(8)]
    public string PasswordHash { get; set; }
    [Required]
    public string Role { get; set; } //Admin, User and Seller

    public string? RefreshToken { get; set; }
}