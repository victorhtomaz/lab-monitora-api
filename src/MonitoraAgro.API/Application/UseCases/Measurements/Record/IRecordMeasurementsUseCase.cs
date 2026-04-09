using MonitoraAgro.API.Application.DTOs.Requests;

namespace MonitoraAgro.API.Application.UseCases.Measurements.Record;

public interface IRecordMeasurementsUseCase
{
    public Task Execute(RecordMeasurementsRequest request);
}