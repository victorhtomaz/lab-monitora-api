using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Repositories;

namespace MonitoraAgro.API.Application.UseCases.Devices.GetSensorDetail;

public class GetSensorDetailUseCase : IGetSensorDetailsUseCase
{
    private readonly IDeviceRepository _deviceRepository;
    private readonly ISensorRepository _sensorRepository;
    public GetSensorDetailUseCase(IDeviceRepository deviceRepository, ISensorRepository sensorRepository)
    {
        _deviceRepository = deviceRepository;
        _sensorRepository = sensorRepository;
    }

    public async Task<SensorDetailResponse> Execute(int deviceId, int sensorId)
    {
        var exist = await _deviceRepository.ExistActiveDeviceWithId(deviceId);
        if (exist is false)
            throw new NotFoundException(ErrorMessages.DEVICE_NOT_FOUND);

        var sensor = await _sensorRepository.GetById(sensorId);
        if (sensor is null)
            throw new NotFoundException(ErrorMessages.SENSOR_NOT_FOUND);

        var summaryResponse = new SensorSummaryResponse(
            Id: sensor.Id,
            DisplayName:sensor.DisplayName,
            Unit: sensor.Unit
            );

        var threshold = sensor.Threshold;
        var thresholdResponse = new ThresholdResponse(
            LowerLimit: threshold.LowerLimit,
            UpperLimit: threshold.UpperLimit,
            NotificationEnabled: threshold.NotificationEnabled,
            NotifiedAt: threshold.NotifiedAt
            );

        return new SensorDetailResponse(
            Summary: summaryResponse,
            Threshold: thresholdResponse
        );
    }
}