using System.Threading.Channels;
using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Services.SensorMeasurement;

namespace MonitoraAgro.API.Infrastructure.Services.SensorMeasurement;

public class SensorMeasurementMemoryQueue : ISensorMeasurementQueue
{
    private readonly Channel<IList<SensorMeasurementInput>> _channel;
    public SensorMeasurementMemoryQueue()
    {
        var options = new BoundedChannelOptions(capacity: 2000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        _channel = Channel.CreateBounded<IList<SensorMeasurementInput>>(options);
    }

    public ValueTask Enqueue(IEnumerable<SensorMeasurementInput> sensorMeasurementInputs)
    {
        var measurements = sensorMeasurementInputs.ToList();

        return _channel.Writer.WriteAsync(measurements);
    }

    public ValueTask<IList<SensorMeasurementInput>> Dequeue()
    {
        return _channel.Reader.ReadAsync();
    }
}
