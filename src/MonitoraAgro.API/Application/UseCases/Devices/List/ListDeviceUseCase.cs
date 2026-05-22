using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.LoggedUser;

namespace MonitoraAgro.API.Application.UseCases.Devices.List;

public class ListDeviceUseCase : IListDeviceUseCase
{
    private readonly IDeviceRepository _repository;
    private readonly ILoggedUser _loggedUser;
    public ListDeviceUseCase(IDeviceRepository repository, ILoggedUser loggedUser)
    {
        _repository = repository;
        _loggedUser = loggedUser;
    }

    public async Task<PagedResponse<DeviceSummaryResponse>> Execute(PagedRequest request)
    {
        // 1- Acessar usuário logado
        var user = await _loggedUser.GetUser();

        // 2- Buscar os dispositivos do usuário logado
        var (devices, totalCount) = await _repository.GetByUserId(user.Id, request.PageNumber, request.PageSize);
        // 3- Mapear os dispositivos para DeviceResponse
        
        var devicesResponse = devices.Select(device => new DeviceSummaryResponse(
            Id: device.Id,
            DisplayName: device.DisplayName,
            IsActive: device.IsActive,
            LastRssi: device.Status.LastRssi,
            LastSoc: device.Status.LastSoc
            )
        );
        // 4- Retornar PagedResponse<DeviceResponse>

        return new PagedResponse<DeviceSummaryResponse>(
                [.. devicesResponse],
                request.PageNumber,
                request.PageSize,
                totalCount
            );
    }
}
