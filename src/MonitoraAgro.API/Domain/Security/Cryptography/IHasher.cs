namespace MonitoraAgro.API.Domain.Security.Cryptography;

public interface IHasher
{
    public string Encrypt(string input);
    public bool IsValid(string input, string hash);
}
