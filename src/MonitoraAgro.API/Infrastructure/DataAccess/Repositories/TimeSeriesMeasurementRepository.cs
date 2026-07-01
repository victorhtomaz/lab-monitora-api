using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;
using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.ValueObjects;

namespace MonitoraAgro.API.Infrastructure.DataAccess.Repositories;

public class TimeSeriesMeasurementRepository(
    IInfluxDBClient influxDbClient, 
    IConfiguration configuration) : ITimeSeriesMeasurementRepository
{
    private readonly string _bucket = configuration.GetValue<string>("Settings:InfluxDB:Bucket") ?? "default";
    private readonly string _organization = configuration.GetValue<string>("Settings:InfluxDB:Organization") ?? "default";
    private const string MEASUREMENT_NAME = "ambiente";
    private const string RESULT_NAME_POINTS = "points";
    private const string RESULT_NAME_GLOBAL_MEAN = "global_mean";
    public async Task InsertSensorsData(Device device, IEnumerable<SensorMeasurementInput> measurements, long timestamp)
    {
        var (recordedAt, precision) = GetTimestamp(timestamp);

        var points = measurements
            .Select(measurement => PointData
                .Measurement(MEASUREMENT_NAME)
                .Tag("user_id", device.Field.UserId.ToString())
                .Tag("field_id", device.FieldId.ToString())
                .Tag("device_id", device.Id.ToString())
                .Field(measurement.InternalKey, measurement.Value)
                .Timestamp(recordedAt, precision));
            
        await influxDbClient.GetWriteApiAsync().WritePointsAsync(points, _bucket, _organization);
    }

    public async Task<TimeSeriesDataResult> GetSensorPoints(int deviceId, string sensorKey, MeasurementPeriod period)
    {
        string aggregateStep = string.IsNullOrEmpty(period.AggregationWindow)
            ? ""
            : $"|> aggregateWindow(every: {period.AggregationWindow}, fn: mean, createEmpty: false)";

        string fluxQuery = $@"
            data = from(bucket: ""{_bucket}"")
                |> range(start: {period.StartDate:o}, stop: {period.EndDate:o})
                |> filter(fn: (r) => r[""_measurement""] == ""{MEASUREMENT_NAME}"")
                |> filter(fn: (r) => r[""device_id""] == ""{deviceId}"")
                |> filter(fn: (r) => r[""_field""] == ""{sensorKey}"")

            data {aggregateStep} |> yield(name: ""{RESULT_NAME_POINTS}"")
            data |> mean() |> yield(name: ""{RESULT_NAME_GLOBAL_MEAN}"")";

        var queryApi = influxDbClient.GetQueryApi();

        var fluxTables = await queryApi.QueryAsync(fluxQuery, _organization);

        return ProcessFluxResults(fluxTables);
    }

    private static (DateTimeOffset Timestamp, WritePrecision Precision) GetTimestamp(long timestamp)
    {
        var isMilliseconds = timestamp > 9_999_999_999;

        return isMilliseconds
            ? (DateTimeOffset.FromUnixTimeMilliseconds(timestamp), WritePrecision.Ms)
            : (DateTimeOffset.FromUnixTimeSeconds(timestamp), WritePrecision.S);
    }

    private static TimeSeriesDataResult ProcessFluxResults(List<FluxTable> fluxTables)
    {
        var points = new List<TimeSeriesPoint>();
        double globalAverage = 0.0;

        foreach (var fluxTable in fluxTables)
        {
            foreach (var fluxRecord in fluxTable.Records)
            {
                string? resultName = fluxRecord.GetValueByKey("result")?.ToString();

                switch (resultName)
                {
                    case RESULT_NAME_POINTS when fluxRecord.GetTime() != null && fluxRecord.GetValue() != null:
                        points.Add(new TimeSeriesPoint(
                            TimeStamp: fluxRecord.GetTime()!.Value.ToDateTimeUtc(),
                            Value: Convert.ToDouble(fluxRecord.GetValue())
                        ));
                        break;

                    case RESULT_NAME_GLOBAL_MEAN when fluxRecord.GetValue() != null:
                        globalAverage = Convert.ToDouble(fluxRecord.GetValue());
                        break;
                }
            }
        }

        return new TimeSeriesDataResult(
            Average: Math.Round(globalAverage, 2),
            Points: points.AsReadOnly()
        );
    }
}
