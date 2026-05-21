using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Repositories;

public interface IDeviceRepository
{
    public Task<Device?> GetById(int deviceId);
    public Task<(IList<Device> Devices, int TotalCount)> GetByUserId(int userId, int page, int pageSize);
    public void Update(Device device);
}
