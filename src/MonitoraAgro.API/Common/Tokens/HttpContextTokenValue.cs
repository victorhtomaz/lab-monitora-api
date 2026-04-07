using MonitoraAgro.API.Domain.Security.Tokens;
using System.Text;

namespace MonitoraAgro.API.Common.Tokens;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public (string identifier, string token) GetCredentials()
    {
        var authenticationHeader = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();

        if (authenticationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
        {
            var base64Credentials = authenticationHeader["Basic ".Length..].Trim();
            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(base64Credentials));
            var parts = decodedCredentials.Split(':', 2);

            return (parts[0], parts.Length > 1 ? parts[1] : string.Empty);
        }

        if (authenticationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            var token = authenticationHeader["Bearer ".Length..].Trim();
            return (string.Empty, token);
        }

        return (string.Empty, authenticationHeader.Trim());
    }
}
