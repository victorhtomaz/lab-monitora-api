namespace MonitoraAgro.API.Domain.Security.Cryptography;

public interface IHasher
{
    public string Encrypt(string password);
    public bool IsValid(string password, string passwordHash);
}
