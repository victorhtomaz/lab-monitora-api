using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Security.Cryptography;
using MonitoraAgro.API.Infrastructure.DataAccess;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;
using MonitoraAgro.API.Infrastructure.DataAccess.Repositories;
using MonitoraAgro.API.Infrastructure.Security.Cryptography;

namespace MonitoraAgro.API.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddRepositories(services);
        AddPasswordEncrypter(services);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MonitoraAgroDbContext>(options =>options.UseNpgsql(connectionString));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
    }

    private static void AddPasswordEncrypter(IServiceCollection services)
    {
        services.AddScoped<IPasswordEncripter, BCryptNet>();
    }
}
