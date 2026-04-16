using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Repositories;

namespace MonitoraAgro.API.Application.UseCases.Measurements.AnalyzeThreshold;

public class AnalyzeMeasurementThresholdUseCase : IAnalyzeMeasurementThresholdUseCase
{
    private readonly ISensorRepository _sensorRepository;
    public async Task Execute(IList<SensorMeasurementInput> sensorMeasurementInputs)
    {
        // Buscar os sensores e seus respectivos limites de alerta
        var sensorIds = sensorMeasurementInputs.Select(s => s.SensorId).ToList();
        var sensors = await _sensorRepository.GetByIds(sensorIds);

        // Chave id, Valor Sensor
        var sensorDictionary = sensors.ToDictionary(s => s.Id);

        // Comparar os valores das medições com os limites de alerta
        // Criar um novo DTO para as informações que enviará no alerta.
        var exceededMeasurements = sensorMeasurementInputs.Where(measurement =>
        {
            if (!sensorDictionary.TryGetValue(measurement.SensorId, out var sensor))
                return false;

            if (sensor == null || sensor.Threshold.NotificationEnabled is false)
                return false;

            return measurement.Value < sensor.Threshold.LowerLimit || measurement.Value > sensor.Threshold.UpperLimit;
        }).ToList();

        // Gerar alertas para as medições que ultrapassarem os limites

        throw new NotImplementedException();
    }
}
