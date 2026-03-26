using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class UserRepository(MonitoraAgroDbContext context) : IUserRepository
{
    public async Task Add(User user)
        => await context.Users.AddAsync(user);

    public async Task<bool> ExistActiveUserWithEmail(string email)
        => await context.Users.AnyAsync(u => u.IsActive && u.Email.Equals(email));
}
