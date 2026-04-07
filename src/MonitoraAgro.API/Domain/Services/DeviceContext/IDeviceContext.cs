using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Services.DeviceContext;

public interface IDeviceContext
{
    Task<Device> GetDevice(); 
}
