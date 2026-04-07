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
    private readonly string _bucket = configuration.GetValue<string>("Settings:InfluxDB:Bucket") ?? "default";
    private readonly string _organization = configuration.GetValue<string>("Settings:InfluxDB:Organization") ?? "default";

    public async Task InsertSensorsData(Device device, IEnumerable<SensorMeasurementInput> measurements, long timestamp)
    {
        var (recordedAt, precision) = GetTimestamp(timestamp);

        var points = measurements
            .Select(measurement => PointData
                .Measurement("ambiente")
                .Tag("user_id", device.Field.UserId.ToString())
                .Tag("field_id", device.FieldId.ToString())
                .Tag("device_id", device.Id.ToString())
                .Field(measurement.InternalKey, measurement.Value)
                .Timestamp(recordedAt, precision));
            
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
