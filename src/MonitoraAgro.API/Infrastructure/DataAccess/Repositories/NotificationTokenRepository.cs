using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class NotificationTokenRepository(MonitoraAgroDbContext context) : INotificationTokenRepository
{
    public async Task Add(NotificationToken notificationToken)
        => await context.NotificationTokens.AddAsync(notificationToken);

    public async Task<NotificationToken?> GetByInstallationId(Guid installationId)
        => await context.NotificationTokens.FirstOrDefaultAsync(nt => nt.InstallationId == installationId);

    public void Update(NotificationToken notificationToken)
        => context.NotificationTokens.Update(notificationToken);

    public async Task<IList<NotificationToken>> GetAllTokenUser(int userId)
        => await context.NotificationTokens
            .Where(nt => nt.UserId == userId)
            .ToListAsync();
}
