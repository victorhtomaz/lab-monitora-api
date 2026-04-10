namespace MonitoraAgro.API.Application.DTOs.Requests;

public record RegisterNotificationTokenRequest(Guid InstallationId, string Token);