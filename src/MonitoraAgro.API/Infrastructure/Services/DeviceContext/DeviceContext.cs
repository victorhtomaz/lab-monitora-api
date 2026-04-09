using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Services.DeviceContext;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;
using System.Security.Claims;

namespace MonitoraAgro.API.Infrastructure.Services.DeviceContext;

public class DeviceContext : IDeviceContext
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly MonitoraAgroDbContext _dbContext;
    public DeviceContext(IHttpContextAccessor httpContextAccessor, MonitoraAgroDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<Device> GetDevice()
    {
        var identifier = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (int.TryParse(identifier, out var deviceId) is false)
            throw new UnauthorizedAccessException("Device is not authenticated.");

        return await _dbContext.Devices
            .AsNoTracking()
            .Include(d => d.Field)
            .FirstAsync(d => d.Id == deviceId);
    }
}
