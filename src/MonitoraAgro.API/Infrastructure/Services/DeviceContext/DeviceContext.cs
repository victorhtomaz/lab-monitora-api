using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Security.Tokens;
using MonitoraAgro.API.Domain.Services.DeviceContext;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.Services.DeviceContext;

public class DeviceContext : IDeviceContext
{
    private readonly ITokenProvider _tokenProvider;
    private readonly MonitoraAgroDbContext _dbContext;
    public DeviceContext(ITokenProvider tokenProvider, MonitoraAgroDbContext dbContext)
    {
        _tokenProvider = tokenProvider;
        _dbContext = dbContext;
    }

    public async Task<Device> GetDevice()
    {
        var (identifier, _) = _tokenProvider.GetCredentials();
        var deviceId = int.Parse(identifier);

        return await _dbContext.Devices
            .AsNoTracking()
            .Include(d => d.Field)
            .FirstAsync(d => d.Id == deviceId);
    }
}
