using InfluxDB.Client.Core.Exceptions;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Domain.Enums;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.ValueObjects;

namespace MonitoraAgro.API.Application.UseCases.Sensors.GetMeasurements;

public class GetSensorMeasurementsUseCase : IGetSensorMeasurementsUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly ITimeSeriesMeasurementRepository _timeSeriesRepository;
    public GetSensorMeasurementsUseCase(
        ISensorRepository sensorRepository, 
        ITimeSeriesMeasurementRepository timeSeriesRepository)
    {
        _sensorRepository = sensorRepository;
        _timeSeriesRepository = timeSeriesRepository;
    }

    public async Task<SensorMeasurementsResponse> Execute(int id, MeasurementFilterRequest filters)
    {
        var sensor = await _sensorRepository.GetById(id);
        if (sensor is null)
            throw new NotFoundException(ErrorMessages.SENSOR_NOT_FOUND);

        var period = filters.StartDate.HasValue
        ? MeasurementPeriod.FromAbsoluteDates(filters.StartDate.Value, filters.EndDate)
        : MeasurementPeriod.FromAvailablePeriod(filters.Period ?? AvailablePeriod.OneDay);

        var queryResult = await _timeSeriesRepository.GetSensorPoints(sensor.DeviceId, sensor.InternalKey, period);

        var pointsResponse = queryResult.Points
            .Select(p => new MeasurementPointResponse(p.TimeStamp, p.Value))
            .ToList();

        return new SensorMeasurementsResponse(
            Unit: sensor.Unit,
            Average: queryResult.Average,
            Points: pointsResponse);
    }
}
