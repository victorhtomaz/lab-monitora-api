using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.UseCases.Measurements.Record;

namespace MonitoraAgro.API.Core.Endpoints;

public static class MeasurementsEndpoints
{
    public static void MapMeasurementsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/measurements")
            .WithTags("Measurements")
            .RequireAuthorization();

        group.MapPost("/", RecordMeasurements);
    }

    private static async Task<IResult> RecordMeasurements(IRecordMeasurementsUseCase useCase, RecordMeasurementsRequest request)
    {
        await useCase.Execute(request);
        
        return TypedResults.Ok();
    }
}