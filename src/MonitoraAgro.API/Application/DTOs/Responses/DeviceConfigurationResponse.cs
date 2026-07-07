namespace MonitoraAgro.API.Application.DTOs.Responses;

public record DeviceConfigurationResponse(IList<SensorThresholdsResponse> SensorThresholds);
