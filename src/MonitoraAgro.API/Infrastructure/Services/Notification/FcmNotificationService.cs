using FirebaseAdmin.Messaging;
using MonitoraAgro.API.Domain.Services.Notification;

namespace MonitoraAgro.API.Infrastructure.Services.Notification;

public class FcmNotificationService : INotificationService
{
    public async Task SendNotificationToAllTokens(string title, string message, IList<string> tokens)
    {
        var androidConfig = new AndroidConfig
        {
            Priority = Priority.High,
            TimeToLive = new TimeSpan(0, 30, 0)
        };

        var notification = new FirebaseAdmin.Messaging.Notification
        {
            Title = title,
            Body = message,
        };

        var messageToSend = new MulticastMessage
        {
            Tokens = tokens.AsReadOnly(),
            Android = androidConfig,
            Notification = notification
        };

        await FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(messageToSend);
    }
}
