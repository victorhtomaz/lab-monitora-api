namespace MonitoraAgro.API.Domain.Security.Cryptography;

public interface IHasher
{
    public string Hash(string input);
    public bool IsValid(string input, string hash);
}
