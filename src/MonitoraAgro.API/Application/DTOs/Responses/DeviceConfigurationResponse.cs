namespace MonitoraAgro.API.Application.DTOs.Responses;

public record DeviceConfigurationResponse(
    uint ReadingIntervalInSeconds, 
    IList<SensorThresholdsResponse> SensorThresholds);
