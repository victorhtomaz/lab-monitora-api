using Microsoft.IdentityModel.Tokens;
using MonitoraAgro.API.Domain.Security.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MonitoraAgro.API.Infrastructure.Security.Tokens.Access.Generator;

public class JwtTokenGenerator : IAccessTokenGenerator
{
    private readonly string _signinKey;
    private readonly uint _expirationTimeInMinutes;
    public JwtTokenGenerator(string signinKey, uint expirationTimeInMinutes)
    {
        _signinKey = signinKey;
        _expirationTimeInMinutes = expirationTimeInMinutes;
    }

    public string Generate(int userId)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString())
        };

        var securityKey = JwtTokenHandler.GetSecurityKey(_signinKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeInMinutes),
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }
}
