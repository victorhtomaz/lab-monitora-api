using MonitoraAgro.API.Application.UseCases.Authentication.DoLogin;
using MonitoraAgro.API.Application.UseCases.Devices.GetById;
using MonitoraAgro.API.Application.UseCases.Devices.GetSensorDetail;
using MonitoraAgro.API.Application.UseCases.Devices.List;
using MonitoraAgro.API.Application.UseCases.Measurements.AnalyzeThreshold;
using MonitoraAgro.API.Application.UseCases.Measurements.Record;
using MonitoraAgro.API.Application.UseCases.Notifications.AddOrUpdateToken;
using MonitoraAgro.API.Application.UseCases.Sensors.GetMeasurements;
using MonitoraAgro.API.Application.UseCases.User.Register;

namespace MonitoraAgro.API.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
       AddUseCases(services);
    }

    private static void AddUseCases(IServiceCollection services)
    {
        services.AddScoped<IDoLoginUseCase, DoLoginUseCase>();

        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();

        services.AddScoped<IRecordMeasurementsUseCase, RecordMeasurementsUseCase>();
        services.AddScoped<IAnalyzeMeasurementThresholdUseCase, AnalyzeMeasurementThresholdUseCase>();

        services.AddScoped<IAddOrUpdateNotificationTokenUseCase, AddOrUpdateNotificationTokenUseCase>();

        services.AddScoped<IListDeviceUseCase, ListDeviceUseCase>();
        services.AddScoped<IGetDeviceByIdUseCase, GetDeviceByIdUseCase>();
        services.AddScoped<IGetSensorDetailsUseCase, GetSensorDetailUseCase>();

        services.AddScoped<IGetSensorMeasurementsUseCase, GetSensorMeasurementsUseCase>();
    }
}
