namespace MonitoraAgro.API.Application.DTOs.Responses;

public record DeviceResponse(int Id, string DisplayName, bool IsActive, int? LastRssi = null, double? LastSoc = null);
