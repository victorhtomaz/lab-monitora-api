using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Repositories;

public interface INotificationTokenRepository
{
    public Task<NotificationToken?> GetByInstallationId(Guid installationId);
    public Task Add(NotificationToken notificationToken);
    public void Update(NotificationToken notificationToken);
    public Task<IList<NotificationToken>> GetAllTokenUser(int userId);
}