using MonitoraAgro.API.Domain.Security.Tokens;

namespace MonitoraAgro.API.Common.Tokens;

public class HttpContextTokenValue : ITokenProvider
{
    private readonly IHttpContextAccessor _contextAccessor;
    public HttpContextTokenValue(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    public string Value()
    {
        var authenticationHeader = _contextAccessor.HttpContext!.Request.Headers.Authorization.ToString();
        return authenticationHeader["Bearer ".Length..].Trim();
    }
}
