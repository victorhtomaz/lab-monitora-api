using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.DeviceContext;
using System.Globalization;
using MonitoraAgro.API.Domain.Services.SensorMeasurement;

namespace MonitoraAgro.API.Application.UseCases.Measurements.Record;

public class RecordMeasurementsUseCase : IRecordMeasurementsUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly ITimeSeriesMeasurementRepository _timeSeriesMeasurementRepository;
    private readonly IDeviceContext _deviceContext;
    private readonly ISensorMeasurementQueue _queue;
    public RecordMeasurementsUseCase(
        ISensorRepository sensorRepository,
        ITimeSeriesMeasurementRepository timeSeriesMeasurementRepository,
        IDeviceContext deviceContext,
        ISensorMeasurementQueue queue)
    {
        _sensorRepository = sensorRepository;
        _timeSeriesMeasurementRepository = timeSeriesMeasurementRepository;
        _deviceContext = deviceContext;
        _queue = queue;
    }

    public async Task Execute(RecordMeasurementsRequest request)
    {
        var device = await _deviceContext.GetDevice();

        var internalKeys = request.data.Keys;
        var sensors = await _sensorRepository.GetByDeviceAndInternalKeys(device, internalKeys);

        if (sensors.Count != internalKeys.Count)
            throw new NotFoundException(ErrorMessages.SENSOR_NOT_FOUND);

        var sensorsData = sensors.Select(sensor =>
        {
            var value = request.data[sensor.InternalKey];
            return new SensorMeasurementInput(sensor.Id, sensor.InternalKey, value);
        }).ToList();

        var analyzeMeasurementData = new AnalyzeMeasurementData(device.Field.UserId, sensorsData);
        
        await _queue.Enqueue(analyzeMeasurementData);

        await _timeSeriesMeasurementRepository.InsertSensorsData(device, sensorsData, request.timestamp);
    }
}