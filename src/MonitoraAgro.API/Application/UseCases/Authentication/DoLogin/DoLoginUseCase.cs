using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Authentication.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    public Task<RegisteredUserResponse> Execute(LoginRequest request)
    {
        throw new NotImplementedException();
    }
}
