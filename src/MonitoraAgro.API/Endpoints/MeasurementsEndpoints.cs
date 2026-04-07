using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.UseCases.Measurements.Record;

namespace MonitoraAgro.API.Endpoints;

public static class MeasurementsEndpoints
{
    public static void MapMeasurementsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/measurements")
            .WithTags("Measurements");

        group.MapPost("/", RecordMeasurements);
    }

    private static async Task<IResult> RecordMeasurements(IRecordMeasurementsUseCase useCase, RecordMeasurementsRequest request)
    {
        await useCase.Execute(request);
        
        return TypedResults.Ok();
    }
}