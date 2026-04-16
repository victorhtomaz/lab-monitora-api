using MonitoraAgro.API.Domain.DTOs;

namespace MonitoraAgro.API.Application.UseCases.Measurements.AnalyzeThreshold;

public interface IAnalyzeMeasurementThresholdUseCase
{
    public Task Execute(IList<SensorMeasurementInput> sensorMeasurementInputs);
}