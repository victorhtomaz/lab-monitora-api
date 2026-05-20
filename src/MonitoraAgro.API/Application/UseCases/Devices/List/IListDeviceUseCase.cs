using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Devices.List;

public interface IListDeviceUseCase
{
    public Task<PagedResponse<DeviceResponse>> Execute(PagedRequest request);
}
