using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Enums;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.Notification;
using System.Text;

namespace MonitoraAgro.API.Application.UseCases.Measurements.AnalyzeThreshold;

public class AnalyzeMeasurementThresholdUseCase : IAnalyzeMeasurementThresholdUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly INotificationTokenRepository _tokenRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly INotificationService _notificationService;
    public AnalyzeMeasurementThresholdUseCase(
        ISensorRepository sensorRepository, 
        INotificationTokenRepository tokenRepository,
        IUnitOfWork unitOfWork,
        INotificationService notificationService)
    {
        _sensorRepository = sensorRepository;
        _tokenRepository = tokenRepository;
        _unitOfWork = unitOfWork;
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

        var exceededMeasurements = new List<(ExceedeMeasurementData Data, Sensor SensorEntity)>();
        foreach (var measurement in sensorMeasurementInputs)
        {
            if (!sensorDictionary.TryGetValue(measurement.SensorId, out var sensor))
                continue;

            if (!sensor.Threshold.NotificationEnabled)
                continue;

            if (sensor.Threshold.NotifiedAt.HasValue &&
                (DateTime.UtcNow - sensor.Threshold.NotifiedAt.Value).TotalMinutes < 5)
                continue;

            if (measurement.Value < sensor.Threshold.LowerLimit)
            {
                var alert = new ExceedeMeasurementData(sensor.DisplayName, sensor.Unit, measurement.Value, LimitType.Lower);
                exceededMeasurements.Add((alert, sensor));
                continue;
            }

            if (measurement.Value > sensor.Threshold.UpperLimit)
            {
                var alert = new ExceedeMeasurementData(sensor.DisplayName, sensor.Unit, measurement.Value, LimitType.Upper);
                exceededMeasurements.Add((alert, sensor));
                continue;
            }
        }

        if (exceededMeasurements.Count == 0)
            return;

        var notificationTokens = await _tokenRepository.GetAllTokenUser(data.UserId);
        if (notificationTokens.Count == 0)
            return;

        var tokens = notificationTokens.Select(t => t.Token).ToList();

        string title;
        var messageBuilder = new StringBuilder();

        if (exceededMeasurements.Count == 1)
        {
            var exceedeMeasurement = exceededMeasurements[0].Data;

            title = $"🚨 Alerta no sensor {exceedeMeasurement.SensorName}";
            messageBuilder.Append($"Medição atual: {exceedeMeasurement.Value:F2}{exceedeMeasurement.SensorUnit}");
        }
        else
        {
            title = $"🚨 {exceededMeasurements.Count} sensores fora do limite!";
            foreach (var (Data, SensorEntity) in exceededMeasurements)
            {
                var alertData = Data;
                messageBuilder.AppendLine($"• {alertData.SensorName}: {alertData.Value:F2}{alertData.SensorUnit}");
            }
        }

        var message = messageBuilder.ToString().TrimEnd();

        await _notificationService.SendNotificationToAllTokens(title, message, tokens);

        await UpdateNotifiedAt([.. exceededMeasurements!.Select(x => x!.SensorEntity)]);
    }

    private async Task UpdateNotifiedAt(IList<Sensor> sensors)
    {
        var currentTime = DateTime.UtcNow;

        foreach (var sensor in sensors)
        {
            sensor.Threshold.NotifiedAt = currentTime;

            _sensorRepository.Update(sensor);
        }

        await _unitOfWork.Commit();
    }
}
