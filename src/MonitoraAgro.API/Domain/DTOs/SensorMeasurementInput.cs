namespace MonitoraAgro.API.Domain.DTOs;

public record SensorMeasurementInput(int SensorId, string InternalKey, double Value);
