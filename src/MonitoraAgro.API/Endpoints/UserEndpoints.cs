using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.UseCases.User.Register;

namespace MonitoraAgro.API.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/users")
            .WithTags("User");

        group.MapPost(string.Empty, CreateUser);
    }

    private static async Task<IResult> CreateUser(IRegisterUserUseCase useCase, RegisterUserRequest request)
    {
        var response = await useCase.Execute(request);

        return TypedResults.Ok(response);
    }
}
