using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.UseCases.Notifications.AddOrUpdateToken;

namespace MonitoraAgro.API.Core.Endpoints;

public static class NotificationEndpoints
{
    public static void MapNotificationsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/notifications")
            .WithTags("Notifications")
            .RequireAuthorization();

        app.MapPost("/tokens", AddOrUpdateNotificationToken)
            .Produces(StatusCodes.Status204NoContent);
    }

    private static async Task<IResult> AddOrUpdateNotificationToken(IAddOrUpdateNotificationTokenUseCase useCase, RegisterNotificationTokenRequest request)
    {
        await useCase.Execute(request);
        
        return TypedResults.NoContent();
    }
}