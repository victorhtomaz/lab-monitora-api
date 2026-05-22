namespace MonitoraAgro.API.Application.DTOs.Responses;

public record DeviceStatusResponse(int? LastRssi, double? LastSoc, DateTime? LastSeenAt);