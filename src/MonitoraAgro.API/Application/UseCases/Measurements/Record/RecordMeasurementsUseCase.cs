using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.DTOs;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Services.DeviceContext;
using MonitoraAgro.API.Domain.Services.SensorMeasurement;

namespace MonitoraAgro.API.Application.UseCases.Measurements.Record;

public class RecordMeasurementsUseCase : IRecordMeasurementsUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IDeviceRepository _deviceRepository;
    private readonly ITimeSeriesMeasurementRepository _timeSeriesMeasurementRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDeviceContext _deviceContext;
    private readonly ISensorMeasurementQueue _queue;
    public RecordMeasurementsUseCase(
        ISensorRepository sensorRepository,
        IDeviceRepository deviceRepository,
        ITimeSeriesMeasurementRepository timeSeriesMeasurementRepository,
        IUnitOfWork unitOfWork,
        IDeviceContext deviceContext,
        ISensorMeasurementQueue queue)
    {
        _sensorRepository = sensorRepository;
        _deviceRepository = deviceRepository;
        _timeSeriesMeasurementRepository = timeSeriesMeasurementRepository;
        _unitOfWork = unitOfWork;
        _deviceContext = deviceContext;
        _queue = queue;
    }

    public async Task Execute(RecordMeasurementsRequest request)
    {
        var device = await _deviceContext.GetDevice();

        var internalKeys = request.Data.Keys.ToList();
        internalKeys.Add("rssi");
        internalKeys.Add("soc");

        var sensors = await _sensorRepository.GetByDeviceAndInternalKeys(device, internalKeys);

        if (sensors.Count != internalKeys.Count)
            throw new NotFoundException(ErrorMessages.SENSOR_NOT_FOUND);

        var sensorsData = sensors.Select(sensor =>
        {
            if (sensor.InternalKey.Equals("rssi"))
               return new SensorMeasurementInput(sensor.Id, sensor.InternalKey, request.Rssi);

            if (sensor.InternalKey.Equals("soc"))
               return new SensorMeasurementInput(sensor.Id, sensor.InternalKey, request.Soc);

            var value = request.Data[sensor.InternalKey];
            return new SensorMeasurementInput(sensor.Id, sensor.InternalKey, value);
        }).ToList();

        var analyzeMeasurementData = new AnalyzeMeasurementData(device.Field.UserId, sensorsData);
        
        await _queue.Enqueue(analyzeMeasurementData);

        await _timeSeriesMeasurementRepository.InsertSensorsData(device, sensorsData, request.Timestamp);

        await UpdateDeviceStatus(device.Id, request);
    }

    private async Task UpdateDeviceStatus(int deviceId, RecordMeasurementsRequest request)
    {
        var device = await _deviceRepository.GetById(deviceId);
        if (device == null)
            return;

        device.Status.LastRssi = request.Rssi;
        device.Status.LastSoc = request.Soc;
        device.Status.LastSeenAt = DateTime.UtcNow;

        _deviceRepository.Update(device);
        await _unitOfWork.Commit();
    }
}