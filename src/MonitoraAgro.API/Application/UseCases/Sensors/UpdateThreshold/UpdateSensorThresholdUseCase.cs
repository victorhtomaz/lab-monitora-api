using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Repositories;

namespace MonitoraAgro.API.Application.UseCases.Sensors.UpdateThreshold;

public class UpdateSensorThresholdUseCase : IUpdateSensorThresholdUseCase
{
    private readonly ISensorRepository _sensorRepository;
    private readonly IUnitOfWork _unitOfWork;
    public UpdateSensorThresholdUseCase(
        ISensorRepository sensorRepository, 
        IUnitOfWork unitOfWork)
    {
        _sensorRepository = sensorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute(int sensorId, UpdateSensorThresholdRequest request)
    {
        Validate(request);

        var sensor = await _sensorRepository.GetForUpdateAsync(sensorId);
        if (sensor is null)
            throw new NotFoundException(ErrorMessages.SENSOR_NOT_FOUND);

        sensor.Threshold.LowerLimit = request.LowerLimit;
        sensor.Threshold.UpperLimit = request.UpperLimit;
        sensor.Threshold.NotificationEnabled = request.NotificationEnabled;

        _sensorRepository.Update(sensor);

        await _unitOfWork.Commit();
    }

    private static void Validate(UpdateSensorThresholdRequest request)
    {
        var validator = new UpdateSensorThresholdRequestValidator();
        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
