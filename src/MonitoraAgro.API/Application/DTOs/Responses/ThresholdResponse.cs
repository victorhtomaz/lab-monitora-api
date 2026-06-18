namespace MonitoraAgro.API.Application.DTOs.Responses;

public record ThresholdResponse(double LowerLimit, double UpperLimit, bool NotificationEnabled, DateTime? NotifiedAt);