using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Sensors.GetMeasurements;

public interface IGetSensorMeasurementsUseCase
{
    public Task<SensorMeasurementsResponse> Execute(int id, MeasurementFilterRequest filters);
}
