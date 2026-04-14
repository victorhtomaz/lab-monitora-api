using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.LoggedUser;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;
using System.Security.Claims;

namespace MonitoraAgro.API.Infrastructure.Services.LoggedUser;

public class LoggedUser : ILoggedUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly MonitoraAgroDbContext _dbContext;
    public LoggedUser(IHttpContextAccessor httpContextAccessor, MonitoraAgroDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<User> GetUser()
    {
        var identifier = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (int.TryParse(identifier, out var userId) is false)
            throw new InvalidLoginException();

        return await _dbContext.Users.AsNoTracking().FirstAsync(user => user.IsActive && user.Id == userId);
    }
}
