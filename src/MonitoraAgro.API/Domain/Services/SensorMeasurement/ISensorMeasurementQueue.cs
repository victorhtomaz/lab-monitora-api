using MonitoraAgro.API.Domain.DTOs;

namespace MonitoraAgro.API.Domain.Services.SensorMeasurement;

public interface ISensorMeasurementQueue
{
    public ValueTask Enqueue(IEnumerable<SensorMeasurementInput> sensorMeasurementInputs);
    public ValueTask<IList<SensorMeasurementInput>> Dequeue();
}