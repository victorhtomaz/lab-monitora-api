using MonitoraAgro.API.Domain.Security.Cryptography;

namespace MonitoraAgro.API.Infrastructure.Security.Cryptography;

public class BCryptNet : IPasswordEncripter
{
    public string Encrypt(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}
