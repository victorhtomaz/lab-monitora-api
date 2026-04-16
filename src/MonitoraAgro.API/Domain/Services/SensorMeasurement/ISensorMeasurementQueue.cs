using MonitoraAgro.API.Domain.DTOs;

namespace MonitoraAgro.API.Domain.Services.SensorMeasurement;

public interface ISensorMeasurementQueue
{
    public ValueTask Enqueue(AnalyzeMeasurementData data);
    public ValueTask<AnalyzeMeasurementData> Dequeue();
}