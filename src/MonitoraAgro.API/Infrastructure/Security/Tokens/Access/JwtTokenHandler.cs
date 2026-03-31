using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace MonitoraAgro.API.Infrastructure.Security.Tokens.Access;

public abstract class JwtTokenHandler
{
    public static SymmetricSecurityKey GetSecurityKey(string signingKey)
    {
        var bytes = Encoding.UTF8.GetBytes(signingKey);
        return new SymmetricSecurityKey(bytes);
    }
}
