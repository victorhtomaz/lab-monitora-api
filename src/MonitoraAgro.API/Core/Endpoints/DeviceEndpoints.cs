using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.UseCases.Devices.GetById;
using MonitoraAgro.API.Application.UseCases.Devices.List;

namespace MonitoraAgro.API.Core.Endpoints;

public static class DeviceEndpoints
{
    public static void MapDeviceEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/devices")
            .WithTags("Devices")
            .RequireAuthorization();

        group.MapGet("/", GetList)
            .Produces<PagedResponse<DeviceSummaryResponse>>()
            .Produces(StatusCodes.Status204NoContent);

        group.MapGet("/{id:int}", GetById)
            .Produces<DeviceDetailResponse>()
            .Produces<ErrorResponse>(StatusCodes.Status404NotFound);
    }

    private static async Task<IResult> GetList(IListDeviceUseCase useCase, [AsParameters] PagedRequest request)
    {
        var response = await useCase.Execute(request);

        if (response.Data.Count == 0)
            return TypedResults.NoContent();

        return TypedResults.Ok(response);
    }

    private static async Task<IResult> GetById(IGetDeviceByIdUseCase useCase, int id)
    {
        var response = await useCase.Execute(id);
        
        return TypedResults.Ok(response);
    }
}
