using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class TimeSeriesMeasurementRepository(
    IInfluxDBClient influxDbClient, 
    IConfiguration configuration) : ITimeSeriesMeasurementRepository
{
    private readonly string _bucket = configuration.GetValue<string>("InfluxDB:Bucket") ?? "default";
    private readonly string _organization = configuration.GetValue<string>("InfluxDB:Organization") ?? "default";

    public async Task InsertSensorsData(Device device, IEnumerable<SensorMeasurementInput> measurements, long timestamp)
    {
        var (recordedAt, precision) = GetTimestamp(timestamp);

        var points = measurements
            .Select(measurement => PointData
                .Measurement("sensor_measurements")
                .Tag("internal_key", measurement.InternalKey)
                .Field("value", measurement.Value)
                .Timestamp(recordedAt, precision))
            .ToList();

        if (points.Count == 0)
            return;

        await influxDbClient.GetWriteApiAsync().WritePointsAsync(points, _bucket, _organization);
    }

    private static (DateTimeOffset Timestamp, WritePrecision Precision) GetTimestamp(long timestamp)
    {
        var isMilliseconds = timestamp > 9_999_999_999;

        return isMilliseconds
            ? (DateTimeOffset.FromUnixTimeMilliseconds(timestamp), WritePrecision.Ms)
            : (DateTimeOffset.FromUnixTimeSeconds(timestamp), WritePrecision.S);
    }
}
