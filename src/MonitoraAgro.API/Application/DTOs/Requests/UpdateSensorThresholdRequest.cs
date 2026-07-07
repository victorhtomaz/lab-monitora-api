namespace MonitoraAgro.API.Application.DTOs.Requests;

public record UpdateSensorThresholdRequest(
    double? LowerLimit, 
    double? UpperLimit,
    bool NotificationEnabled);
