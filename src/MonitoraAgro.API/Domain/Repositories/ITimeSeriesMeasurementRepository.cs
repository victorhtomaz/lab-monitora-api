using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Entities;
using MonitoraAgro.API.Domain.ValueObjects;

namespace MonitoraAgro.API.Domain.Repositories;

public interface ITimeSeriesMeasurementRepository
{
    public Task InsertSensorsData(Device device, IEnumerable<SensorMeasurementInput> measurements, long timestamp);
    public Task<TimeSeriesDataResult> GetSensorPoints(int deviceId, string sensorKey, MeasurementPeriod period);
}
