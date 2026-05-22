namespace MonitoraAgro.API.Application.DTOs.Responses;

public record DeviceSummaryResponse(int Id, string DisplayName, bool IsActive, int? LastRssi = null, double? LastSoc = null);
