using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class SensorRepository(MonitoraAgroDbContext context) : ISensorRepository
{
    public async Task<IList<Sensor>> GetByDeviceAndInternalKeys(
        Device device, 
        IEnumerable<string> internalKeys)
        => await context.Sensors
            .AsNoTracking()
            .Where(sensor => sensor.DeviceId == device.Id && internalKeys.Contains(sensor.InternalKey))
            .ToListAsync();

    public async Task<IList<Sensor>> GetByIds(IEnumerable<int> ids)
        => await context.Sensors
            .AsNoTracking()
            .Include(sensor => sensor.Threshold)
            .Where(sensor => ids.Contains(sensor.Id))
            .ToListAsync();
}
