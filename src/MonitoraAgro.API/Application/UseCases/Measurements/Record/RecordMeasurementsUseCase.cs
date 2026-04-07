using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.DeviceContext;
using System.Globalization;

namespace MonitoraAgro.API.Application.UseCases.Measurements.Record;

public class RecordMeasurementsUseCase : IRecordMeasurementsUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly ITimeSeriesMeasurementRepository _timeSeriesMeasurementRepository;
    private readonly IDeviceContext _deviceContext;
    public RecordMeasurementsUseCase(
        ISensorRepository sensorRepository,
        ITimeSeriesMeasurementRepository timeSeriesMeasurementRepository,
        IDeviceContext deviceContext)
    {
        _sensorRepository = sensorRepository;
        _timeSeriesMeasurementRepository = timeSeriesMeasurementRepository;
        _deviceContext = deviceContext;
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
            //var doubleValue = Convert.ToDouble(value, CultureInfo.InvariantCulture);
            return new SensorMeasurementInput(sensor.InternalKey, value);
        });

        await _timeSeriesMeasurementRepository.InsertSensorsData(device, sensorsData, request.timestamp);
    }
}