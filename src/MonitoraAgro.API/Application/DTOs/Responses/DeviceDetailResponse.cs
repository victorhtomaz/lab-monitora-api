namespace MonitoraAgro.API.Application.DTOs.Responses;

public record DeviceDetailResponse(
    int Id,
    string DisplayName, 
    uint ReadingIntervalInSeconds,
    DeviceStatusResponse Status,
    IList<SensorSummaryResponse> Sensors
);