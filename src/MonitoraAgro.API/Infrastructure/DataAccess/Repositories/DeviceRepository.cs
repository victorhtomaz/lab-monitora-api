using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class DeviceRepository(MonitoraAgroDbContext context) : IDeviceRepository
{
    public async Task<Device?> GetById(int deviceId)
        => await context.Devices.Include(d => d.Status).FirstOrDefaultAsync(d => d.Id == deviceId);

    public void Update(Device device)
        => context.Devices.Update(device);
}
