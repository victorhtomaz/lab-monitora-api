using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Entities;

namespace MonitoraAgro.API.Domain.Repositories;

public interface ITimeSeriesMeasurementRepository
{
    public Task InsertSensorsData(Device device, IEnumerable<SensorMeasurementInput> measurements, long timestamp);
}
