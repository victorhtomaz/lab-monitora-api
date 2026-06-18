using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Devices.GetSensorDetail;

public interface IGetSensorDetailsUseCase
{
    public Task<SensorDetailResponse> Execute(int deviceId, int sensorId);
}