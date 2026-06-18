using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Repositories;

namespace MonitoraAgro.API.Application.UseCases.Devices.GetById;

public class GetDeviceByIdUseCase : IGetDeviceByIdUseCase
{
    private readonly IDeviceRepository _repository;
    public GetDeviceByIdUseCase(IDeviceRepository repository)
    {
        _repository = repository;
    }
    public async Task<DeviceDetailResponse> Execute(int id)
    {
        var device = await _repository.GetById(id);

        if (device is null)
            throw new NotFoundException(ErrorMessages.DEVICE_NOT_FOUND);

        return new DeviceDetailResponse(
                Id: device.Id,
                DisplayName: device.DisplayName,
                ReadingIntervalInSeconds: device.ReadingIntervalInSeconds,
                Status: new DeviceStatusResponse(
                    LastRssi: device.Status.LastRssi,
                    LastSoc: device.Status.LastSoc,
                    LastSeenAt: device.Status.LastSeenAt
                ),
                Sensors: [.. device.Sensors.Select(s => new SensorSummaryResponse(
                    Id: s.Id,
                    DisplayName: s.DisplayName,
                    Unit: s.Unit
                ))]
            );
    }
}
