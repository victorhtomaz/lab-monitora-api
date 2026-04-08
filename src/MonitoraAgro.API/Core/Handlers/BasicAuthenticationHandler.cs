using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Security.Cryptography;
using System.Security.Claims;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;

namespace MonitoraAgro.API.Core.Handlers;

public class BasicAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder,
    IDeviceRepository deviceRepository,
    IHasher hasher)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    private readonly IDeviceRepository _deviceRepository = deviceRepository;
    private readonly IHasher _hasher = hasher;

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (string.IsNullOrEmpty(Request.Headers.Authorization))
            return AuthenticateResult.Fail("Header de autorização não encontrado.");

        try
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers.Authorization!);

            if (!"Basic".Equals(authHeader.Scheme, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(authHeader.Parameter))
                return AuthenticateResult.Fail("Header de autorização inválido.");

            var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter!));
            var parts = decodedCredentials.Split(':', 2);
            if (parts.Length != 2)
                return AuthenticateResult.Fail("Header de autorização inválido.");

            var deviceId = parts[0];
            var token = parts[1];

            var device = await _deviceRepository.GetById(int.Parse(deviceId));
            if (device is null /*|| _hasher.IsValid(token, device.TokenHash) is false*/)
                return AuthenticateResult.Fail("Token de autenticação inválido.");

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, device.Id.ToString())
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch
        {
            return AuthenticateResult.Fail("Header de autorização inválido.");
        }
    }
}