namespace MonitoraAgro.API.Application.DTOs.Responses;

public record SensorMeasurementsResponse(
    string Unit,
    double Average,
    IReadOnlyCollection<MeasurementPointResponse> Points);
