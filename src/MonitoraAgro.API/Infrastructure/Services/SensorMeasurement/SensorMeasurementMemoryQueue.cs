using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Services.SensorMeasurement;
using System.Threading.Channels;

namespace MonitoraAgro.API.Infrastructure.Services.SensorMeasurement;

public class SensorMeasurementMemoryQueue : ISensorMeasurementQueue
{
    private readonly Channel<AnalyzeMeasurementData> _channel;
    public SensorMeasurementMemoryQueue()
    {
        var options = new BoundedChannelOptions(capacity: 2000)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = false
        };

        _channel = Channel.CreateBounded<AnalyzeMeasurementData>(options);
    }

    public ValueTask Enqueue(AnalyzeMeasurementData data)
    {
        return _channel.Writer.WriteAsync(data);
    }

    public ValueTask<AnalyzeMeasurementData> Dequeue()
    {
        return _channel.Reader.ReadAsync();
    }
}
