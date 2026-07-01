namespace MonitoraAgro.API.Application.DTOs.Responses;

public record SensorMeasurementsResponse(
    string Unit, 
    IReadOnlyCollection<MeasurementPointResponse> Points);
