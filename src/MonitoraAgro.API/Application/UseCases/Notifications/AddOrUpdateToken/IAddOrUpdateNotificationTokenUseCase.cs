using MonitoraAgro.API.Application.DTOs.Requests;

namespace MonitoraAgro.API.Application.UseCases.Notifications.AddOrUpdateToken;

public interface IAddOrUpdateNotificationTokenUseCase
{
    public Task Execute(RegisterNotificationTokenRequest request);
}