using MonitoraAgro.API.Application.DTOs.Requests;

namespace MonitoraAgro.API.Application.UseCases.Measurements.Record;

public class RecordMeasurementsUseCase : IRecordMeasurementsUseCase
{
    public Task Execute(RecordMeasurementsRequest request)
    {
        // 1- Pegar dispositivo ligado
        // 2- Checar se sensor/medida está salvo
        // 3- Caso não esteja salvo. Salvar
        // 4- Salvar dados no InfluxDB
        
        throw new NotImplementedException();
    }
}