namespace MonitoraAgro.API.Domain.Services.Notification;

public interface INotificationService
{
    public Task SendNotificationToAllTokens(string title, string message, IList<string> tokens);
}