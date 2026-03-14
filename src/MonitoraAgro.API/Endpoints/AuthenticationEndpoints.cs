using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.UseCases.Authentication.DoLogin;

namespace MonitoraAgro.API.Endpoints;

public static class AuthenticationEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/auth")
            .WithTags("Auth");

        group.MapPost("/login", DoLogin)
            .Produces<ErrorResponse>(StatusCodes.Status401Unauthorized);
    }

    private static async Task<IResult> DoLogin(IDoLoginUseCase useCase, LoginRequest request)
    {
        var response = await useCase.Execute(request);

        return TypedResults.Ok(response);
    }
}
