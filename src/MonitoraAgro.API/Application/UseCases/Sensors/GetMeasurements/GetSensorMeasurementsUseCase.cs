using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Sensors.GetMeasurements;

public class GetSensorMeasurementsUseCase : IGetSensorMeasurementsUseCase
{
    public Task<SensorMeasurementsResponse> Execute(int id, MeasurementFilterRequest filter)
    {
        throw new NotImplementedException();

        // Buscar sensor pelo Id

        // Configurar filtro de data e hora

        // Buscar medições do sensor com base no filtro

        // Retornar resposta com as medições
    }
}
