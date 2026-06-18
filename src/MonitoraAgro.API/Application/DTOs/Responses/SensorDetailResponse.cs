namespace MonitoraAgro.API.Application.DTOs.Responses;

public record SensorDetailResponse(SensorSummaryResponse Summary, ThresholdResponse Threshold);