using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Repositories;

public interface IUserRepository
{
    public Task Add(User user);
    public Task<bool> ExistActiveUserWithEmail(string email);
}
