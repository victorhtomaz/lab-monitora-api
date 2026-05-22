using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class DeviceRepository(MonitoraAgroDbContext context) : IDeviceRepository
{
    public async Task<Device?> GetById(int deviceId)
        => await context.Devices
            .Include(d => d.Status)
            .Include(d => d.Sensors)
            .FirstOrDefaultAsync(d => d.Id == deviceId);

    public async Task<(IList<Device> Devices, int TotalCount)> GetByUserId(int userId, int page, int pageSize)
    {
        var query = context.Devices
            .AsNoTracking()
            .Where(d => d.Field.UserId == userId);

        var totalCount = await query.CountAsync();

        var devices = await query
            .Include(d => d.Status)
            .OrderByDescending(d => d.Status.LastSeenAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (devices, totalCount);
    }

    public void Update(Device device)
        => context.Devices.Update(device);
}
