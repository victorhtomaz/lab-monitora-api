using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Repositories;

public interface IDeviceRepository
{
    public Task<Device?> GetById(int deviceId);
    public void Update(Device device);
}
