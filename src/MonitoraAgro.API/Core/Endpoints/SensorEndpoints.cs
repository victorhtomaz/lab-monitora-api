using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.UseCases.Sensors.GetMeasurements;
using MonitoraAgro.API.Application.UseCases.Sensors.UpdateThreshold;

namespace MonitoraAgro.API.Core.Endpoints;

public static class SensorEndpoints 
{
    public static void MapSensorEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/sensors")
            .WithTags("Sensors")
            .RequireAuthorization();

        group.MapGet("/{id:int}/measurements", GetMeasurements)
            .Produces<SensorMeasurementsResponse>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);

        group.MapPut("/{id:int}/thresholds", UpdateSensorThreshold)
            .Produces(StatusCodes.Status200OK)
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetMeasurements(
        IGetSensorMeasurementsUseCase useCase,
        int id, 
        [AsParameters] MeasurementFilterRequest filters)
    {
        var response = await useCase.Execute(id, filters);

        if (response.Points.Count == 0)
            return TypedResults.NoContent();

        return TypedResults.Ok(response);
    }

    private static async Task<IResult> UpdateSensorThreshold(
        IUpdateSensorThresholdUseCase useCase,
        int id,
        UpdateSensorThresholdRequest request)
    {
        await useCase.Execute(id, request);

        return TypedResults.Ok();
    }
}
