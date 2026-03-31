namespace MonitoraAgro.API.Application.DTOs.Requests;

public record RecordMeasurementsRequest(Dictionary<string, object> data, long timestamp);