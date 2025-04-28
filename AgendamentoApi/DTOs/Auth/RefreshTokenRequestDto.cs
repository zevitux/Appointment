using System.ComponentModel.DataAnnotations;

namespace AgendamentoApi.DTOs.Auth;

public class RefreshTokenRequestDto
{
    [Required(ErrorMessage = "Id of user is required")]
    public Guid UserId { get; set; }
    [Required(ErrorMessage = "Refresh Token is required")]
    public string RefreshToken { get; set; }
}