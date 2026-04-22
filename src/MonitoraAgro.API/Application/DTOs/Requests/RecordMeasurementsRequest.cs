namespace MonitoraAgro.API.Application.DTOs.Requests;

public record RecordMeasurementsRequest(Dictionary<string, double> Data, long Timestamp);