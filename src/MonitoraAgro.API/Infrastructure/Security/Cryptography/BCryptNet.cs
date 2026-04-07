using MonitoraAgro.API.Domain.Security.Cryptography;

namespace MonitoraAgro.API.Infrastructure.Security.Cryptography;

public class BCryptNet : IHasher
{
    public string Hash(string input) => BCrypt.Net.BCrypt.HashPassword(input);

    public bool IsValid(string input, string hash) => BCrypt.Net.BCrypt.Verify(input, hash);
}
