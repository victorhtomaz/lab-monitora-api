using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using InfluxDB.Client;
using Microsoft.EntityFrameworkCore;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Security.Cryptography;
using MonitoraAgro.API.Domain.Security.Tokens;
using MonitoraAgro.API.Domain.Services.DeviceContext;
using MonitoraAgro.API.Domain.Services.LoggedUser;
using MonitoraAgro.API.Domain.Services.Notification;
using MonitoraAgro.API.Domain.Services.SensorMeasurement;
using MonitoraAgro.API.Infrastructure.DataAccess;
using MonitoraAgro.API.Infrastructure.DataAccess.PostgreSQL;
using MonitoraAgro.API.Infrastructure.DataAccess.Repositories;
using MonitoraAgro.API.Infrastructure.Security.Cryptography;
using MonitoraAgro.API.Infrastructure.Security.Tokens.Access.Generator;
using MonitoraAgro.API.Infrastructure.Services.DeviceContext;
using MonitoraAgro.API.Infrastructure.Services.LoggedUser;
using MonitoraAgro.API.Infrastructure.Services.Notification;
using MonitoraAgro.API.Infrastructure.Services.SensorMeasurement;

namespace MonitoraAgro.API.Infrastructure;

public static class DependencyInjection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddDbContext(services, configuration);
        AddInfluxDb(services, configuration);
        AddRepositories(services);
        AddPasswordEncrypter(services);
        AddServices(services);
        AddTokens(services, configuration);
        AddFirebaseService(services, configuration);
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<MonitoraAgroDbContext>(options =>options.UseNpgsql(connectionString));
    }
    
    private static void AddInfluxDb(IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration.GetValue<string>("Settings:InfluxDB:Url") ?? "http://localhost:8086";
        var token = configuration.GetValue<string>("Settings:InfluxDB:Token") ?? string.Empty;

        services.AddSingleton<IInfluxDBClient>(_ => new InfluxDBClient(url, token));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IDeviceRepository, DeviceRepository>();
        services.AddScoped<ISensorRepository, SensorRepository>();
        services.AddScoped<ITimeSeriesMeasurementRepository, TimeSeriesMeasurementRepository>();
        services.AddScoped<INotificationTokenRepository, NotificationTokenRepository>();
    }

    private static void AddPasswordEncrypter(IServiceCollection services)
    {
        services.AddScoped<IHasher, BCryptNet>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<ISensorMeasurementQueue, SensorMeasurementMemoryQueue>();
        services.AddScoped<IDeviceContext, DeviceContext>();
        services.AddScoped<ILoggedUser, LoggedUser>();
        services.AddScoped<INotificationService, FcmNotificationService>();
    }

    private static void AddTokens(IServiceCollection services, IConfiguration configuration)
    {
        var signinKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");
        var expirationTimeInMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeInMinutes");

        services.AddScoped<IAccessTokenGenerator>(o => new JwtTokenGenerator(signinKey!, expirationTimeInMinutes));
    }

    private static void AddFirebaseService(IServiceCollection services, IConfiguration configuration)
    {
        var firebaseProjectId = configuration.GetValue<string>("Settings:Firebase:ProjectId");
        var credentialPath = configuration.GetValue<string>("Settings:Firebase:CredentialFilePath");

        FirebaseApp.Create(new AppOptions
        {
            ProjectId = firebaseProjectId,
            Credential = CredentialFactory.FromFile<ServiceAccountCredential>(credentialPath!).ToGoogleCredential()
        });
    }
}
