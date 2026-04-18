namespace MonitoraAgro.API.Domain.DTOs;

public record AnalyzeMeasurementData(int UserId, IList<SensorMeasurementInput> SensorMeasurementInputs);