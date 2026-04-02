using FluentValidation.Results;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.DTOs.Responses;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Application.Errors.Exceptions;
using MonitoraAgro.API.Domain.Repositories;
using MonitoraAgro.API.Domain.Security.Cryptography;
using MonitoraAgro.API.Domain.Security.Tokens;

namespace MonitoraAgro.API.Application.UseCases.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHasher _hasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    public RegisterUserUseCase(IUserRepository userRepository, 
        IUnitOfWork unitOfWork, 
        IHasher hasher,
        IAccessTokenGenerator accessTokenGenerator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _hasher = hasher;
        _accessTokenGenerator = accessTokenGenerator;
    }

    public async Task<RegisteredUserResponse> Execute(RegisterUserRequest request)
    {
        await Validate(request);
        
        var passwordHash = _hasher.Encrypt(request.Password);

        var user = new Domain.Entities.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = passwordHash,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
        
        await _userRepository.Add(user);
        await _unitOfWork.Commit();

        var accessToken = _accessTokenGenerator.Generate(user.Id);

        return new RegisteredUserResponse($"{user.FirstName} {user.LastName}", accessToken);
    }

    private async Task Validate(RegisterUserRequest request)
    {
        var validator = new RegisterUserValidator();
        var result = await validator.ValidateAsync(request);

        var emailExist = await _userRepository.ExistActiveUserWithEmail(request.Email);

        if (emailExist)
            result.Errors.Add(new ValidationFailure(string.Empty, ErrorMessages.EMAIL_ALREADY_REGISTERED));

        if (result.IsValid is false)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errors);
        }
    }
}
