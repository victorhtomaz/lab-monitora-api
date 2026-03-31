using FluentValidation;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Security.Cryptography;
using MonitoraAgro.API.Domain.Security.Tokens;

namespace MonitoraAgro.API.Application.UseCases.Authentication.DoLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    public DoLoginUseCase(IUserRepository userRepository, 
        IPasswordEncripter passwordEncripter, 
        IAccessTokenGenerator accessTokenGenerator)
    {
        _userRepository = userRepository;
        _passwordEncripter = passwordEncripter;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<RegisteredUserResponse> Execute(LoginRequest request)
    {
        Validate(request);

        var user = await _userRepository.GetByEmail(request.Email);
        if (user is null || _passwordEncripter.IsValid(request.Password, user.PasswordHash) is false)
            throw new InvalidLoginException();

        var accessToken = _accessTokenGenerator.Generate(user.Id);

        return new RegisteredUserResponse($"{user.FirstName} {user.LastName}", accessToken);
    }

    private static void Validate(LoginRequest request)
    {
        var validator = new DoLoginValidator();
        var result = validator.Validate(request);

        if (result.IsValid is false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
