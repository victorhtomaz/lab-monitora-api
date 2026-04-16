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
    public async Task Execute(AnalyzeMeasurementData data)
    {
        // Buscar os sensores e seus respectivos limites de alerta
        var sensorMeasurementInputs = data.SensorMeasurementInputs;
        
        var sensorIds = sensorMeasurementInputs.Select(s => s.SensorId).ToList();
        var sensors = await _sensorRepository.GetByIds(sensorIds);

        // Chave id, Valor Sensor
        var sensorDictionary = sensors.ToDictionary(s => s.Id);

        // Comparar os valores das medições com os limites de alerta

        var exceededMeasurements = sensorMeasurementInputs.Select(measurement =>
        {
            var sensor = sensorDictionary.GetValueOrDefault(measurement.SensorId);

            if (sensor!.Threshold.NotificationEnabled is false) 
                return null;
            
            if (measurement.Value < sensor.Threshold.LowerLimit)
                return new ExceedeMeasurementData(sensor.DisplayName , sensor.Unit , measurement.Value, LimitType.Lower);

            if (measurement.Value > sensor.Threshold.UpperLimit)
                return new ExceedeMeasurementData(sensor.DisplayName, sensor.Unit, measurement.Value, LimitType.Upper);

            return null;
        }).Where(e => e is not null)
        .ToList();

    
        // Pegar Tokens do usuário
        var notificationTokens = await _tokenRepository.GetAllTokenUser(data.UserId);
        if (!notificationTokens.Any())
            return;

        var tokens = notificationTokens.Select(t => t.Token).ToList();

        var title = $"{exceededMeasurements.Count} Alerta(s) de sensor: ";
        var message = "O limite foi excedido em: \n";
        foreach (var exceededMeasurement in exceededMeasurements)
        {
            if (exceededMeasurement is null)
                return;

            message += $"{exceededMeasurement.SensorName} - {exceededMeasurement}{exceededMeasurement.SensorUnit}";
        }
        // Gerar alertas para as medições que ultrapassarem os limites

        await _notificationService.SendNotificationToAllTokens(title, message, tokens);
    }
}
