using MonitoraAgro.API.Domain.Enums;

namespace MonitoraAgro.API.Domain.DTOs;

public record ExceedeMeasurementData(string SensorName, string SensorUnit, double Value, LimitType Type);