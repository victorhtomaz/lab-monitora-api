namespace MonitoraAgro.API.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    public string Generate(int userId);
}
