using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.Authentication.DoLogin;

public interface IDoLoginUseCase
{
    public Task<RegisteredUserResponse> Execute(LoginRequest request);
}
