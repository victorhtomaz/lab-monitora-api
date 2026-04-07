using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.Extensions.Options;

namespace MonitoraAgro.API.Handlers;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization))
            return AuthenticateResult.Fail("Header de autorização não encontrado.");

        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization!);
        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter!));
        var parts = decodedCredentials.Split(':', 2);
        var deviceId = parts[0];
        var token = parts[1];
        try
        {
            return AuthenticateResult.Success(null);
        }
        catch
        {
            return AuthenticateResult.Fail("Header de autorização inválido.");
        }
    }
}