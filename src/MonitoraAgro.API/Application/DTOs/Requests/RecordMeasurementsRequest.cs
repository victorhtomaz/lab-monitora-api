namespace MonitoraAgro.API.Application.DTOs.Requests;

public record RecordMeasurementsRequest(Dictionary<string, double> data, long timestamp);