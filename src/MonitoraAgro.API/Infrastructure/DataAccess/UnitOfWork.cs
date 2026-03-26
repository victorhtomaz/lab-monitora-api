using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;

namespace MonitoraAgro.API.Infrastructure.DataAccess;

public class UnitOfWork(MonitoraAgroDbContext context) : IUnitOfWork
{
    public async Task Commit() => await context.SaveChangesAsync();
}
