namespace MonitoraAgro.API.Application.DTOs.Responses;

public record SensorThresholdsResponse(
    string InternalKey,
    double? LowerLimit, 
    double? UpperLimit, 
    bool NotificationEnabled
    );
