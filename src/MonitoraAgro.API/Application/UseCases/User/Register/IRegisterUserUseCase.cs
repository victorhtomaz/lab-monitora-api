using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;

namespace MonitoraAgro.API.Application.UseCases.User.Register;

public interface IRegisterUserUseCase
{
    public Task<RegisteredUserResponse> Execute(RegisterUserRequest request);
}
