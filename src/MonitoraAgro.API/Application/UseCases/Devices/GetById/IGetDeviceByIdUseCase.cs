using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Devices.GetById;

public interface IGetDeviceByIdUseCase
{
    public Task<DeviceDetailResponse> Execute(int id);
}
