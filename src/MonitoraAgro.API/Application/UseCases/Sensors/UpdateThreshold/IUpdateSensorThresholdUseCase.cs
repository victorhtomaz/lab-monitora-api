using MonitoraAgro.API.Application.DTOs.Requests;

namespace MonitoraAgro.API.Application.UseCases.Sensors.UpdateThreshold;

public interface IUpdateSensorThresholdUseCase
{
    public Task Execute(int sensorId, UpdateSensorThresholdRequest request);
}
