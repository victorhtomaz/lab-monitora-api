namespace MonitoraAgro.API.Domain.Repositories;

public interface IUnitOfWork
{
    public Task Commit();
}
