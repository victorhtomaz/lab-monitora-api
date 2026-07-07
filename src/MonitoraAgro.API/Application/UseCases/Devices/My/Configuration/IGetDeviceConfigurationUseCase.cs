using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Devices.My.Configuration;

public interface IGetDeviceConfigurationUseCase
{
    public Task<DeviceConfigurationResponse> Execute();
}
