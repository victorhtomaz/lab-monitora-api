using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.UseCases.User.Register;

namespace MonitoraAgro.API.Core.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users")
            .WithTags("User");

        group.MapPost(string.Empty, CreateUser)
            .Produces<RegisteredUserResponse>(StatusCodes.Status201Created);
    }

    private static async Task<IResult> CreateUser(IRegisterUserUseCase useCase, RegisterUserRequest request)
    {
        var response = await useCase.Execute(request);

        return TypedResults.Created(string.Empty, response);
    }
}
