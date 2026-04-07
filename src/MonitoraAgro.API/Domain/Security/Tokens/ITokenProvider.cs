namespace MonitoraAgro.API.Domain.Security.Tokens;

public interface ITokenProvider
{
    public (string identifier, string token) GetCredentials();
}
