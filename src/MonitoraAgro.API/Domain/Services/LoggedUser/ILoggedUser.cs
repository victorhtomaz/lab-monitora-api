using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Services.LoggedUser;

public interface ILoggedUser
{
    Task<User> GetUser();
}
