using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.DeviceContext;

namespace MonitoraAgro.API.Application.UseCases.Devices.My.Configuration;

public class GetDeviceConfigurationUseCase : IGetDeviceConfigurationUseCase
{
    private readonly IDeviceContext deviceContext;
    private readonly IDeviceRepository deviceRepository;
    public GetDeviceConfigurationUseCase(
        IDeviceContext deviceContext, 
        IDeviceRepository deviceRepository)
    {
        this.deviceContext = deviceContext;
        this.deviceRepository = deviceRepository;
    }

    public async Task<DeviceConfigurationResponse> Execute()
    {
        var device = await deviceContext.GetDevice();

        var deviceConfiguration = await deviceRepository.GetConfigurationById(device.Id);

        var thresholdsResponse = deviceConfiguration!.Sensors
            .Select(sensor => new SensorThresholdsResponse(
                InternalKey: sensor.InternalKey,
                LowerLimit: sensor.Threshold.LowerLimit,
                UpperLimit: sensor.Threshold.UpperLimit,
                NotificationEnabled: sensor.Threshold.NotificationEnabled
                )).ToList();


        return new DeviceConfigurationResponse(
            ReadingIntervalInSeconds: deviceConfiguration.ReadingIntervalInSeconds,
            SensorThresholds: thresholdsResponse
            );
    }
}
