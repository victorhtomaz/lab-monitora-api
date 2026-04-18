using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Enums;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.Notification;

namespace MonitoraAgro.API.Application.UseCases.Measurements.AnalyzeThreshold;

public class AnalyzeMeasurementThresholdUseCase : IAnalyzeMeasurementThresholdUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly INotificationTokenRepository _tokenRepository;
    private readonly INotificationService _notificationService;
    public AnalyzeMeasurementThresholdUseCase(
        ISensorRepository sensorRepository, 
        INotificationTokenRepository tokenRepository, 
        INotificationService notificationService)
    {
        _sensorRepository = sensorRepository;
        _tokenRepository = tokenRepository;
        _notificationService = notificationService;
    }

    public async Task Execute(AnalyzeMeasurementData data)
    {
        var sensorMeasurementInputs = data.SensorMeasurementInputs;
        if (sensorMeasurementInputs.Count == 0)
            return;

        var sensorIds = sensorMeasurementInputs.Select(s => s.SensorId).ToList();
        var sensors = await _sensorRepository.GetByIds(sensorIds);

        var sensorDictionary = sensors.ToDictionary(s => s.Id);


        var exceededMeasurements = sensorMeasurementInputs
            .Select(measurement =>
            {
                if (!sensorDictionary.TryGetValue(measurement.SensorId, out var sensor))
                    return null;

                if (!sensor.Threshold.NotificationEnabled)
                    return null;

                if (measurement.Value < sensor.Threshold?.LowerLimit)
                    return new ExceedeMeasurementData(sensor.DisplayName, sensor.Unit, measurement.Value, LimitType.Lower);

                if (measurement.Value > sensor.Threshold?.UpperLimit)
                    return new ExceedeMeasurementData(sensor.DisplayName, sensor.Unit, measurement.Value, LimitType.Upper);

                return null;
            })
            .OfType<ExceedeMeasurementData>()
            .ToList();

        if (exceededMeasurements.Count == 0)
            return;

        var notificationTokens = await _tokenRepository.GetAllTokenUser(data.UserId);
        if (notificationTokens.Count == 0)
            return;

        var tokens = notificationTokens.Select(t => t.Token).ToList();

        var title = $"{exceededMeasurements.Count} Alerta(s) de sensor: ";
        var message = "O limite foi excedido em: \n";
        foreach (var exceededMeasurement in exceededMeasurements)
        {
            message += $"{exceededMeasurement.SensorName} - {exceededMeasurement.Value}{exceededMeasurement.SensorUnit}\n";
        }

        await _notificationService.SendNotificationToAllTokens(title, message, tokens);
    }
}
