using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Repositories;

public interface ISensorRepository
{
    public Task<IList<Sensor>> GetByDeviceAndInternalKeys(Device device, IEnumerable<string> internalKeys);
    public Task<IList<Sensor>> GetByIds(IEnumerable<int> ids);
    public Task<Sensor?> GetById(int id);
}
