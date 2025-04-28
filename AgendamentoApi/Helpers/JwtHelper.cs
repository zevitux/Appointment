using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AgendamentoApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace AgendamentoApi.Helpers;

public class JwtHelper
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<JwtHelper> _logger;

    public JwtHelper(IConfiguration configuration, ILogger<JwtHelper> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public string CreateToken(List<Claim> claims)
    {
        var tokenKey = _configuration.GetValue<string>("AppSettings:Token")
                       ?? throw new Exception("Token key is not set in appsettings.json");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: _configuration.GetValue<string>("AppSettings:Issuer"),
            audience: _configuration.GetValue<string>("AppSettings:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor); //Returns the token as a string
    }
}