using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.UseCases.Sensors.GetMeasurements;

namespace MonitoraAgro.API.Core.Endpoints;

public static class SensorEndpoints 
{
    public static void MapSensorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/sensors")
            .WithTags("Sensors");

        group.MapGet("/{id:int}/measurements", GetMeasurements)
            .Produces<SensorMeasurementsResponse>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetMeasurements(
        IGetSensorMeasurementsUseCase useCase,
        int id, 
        [AsParameters] MeasurementFilterRequest filter)
    {
        var response = await useCase.Execute(id, filter);

        if (response.Points.Count == 0)
            return TypedResults.NoContent();

        return TypedResults.Ok(response);
    }
}
