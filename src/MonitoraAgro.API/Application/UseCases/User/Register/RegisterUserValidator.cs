using FluentValidation;
using MonitoraAgro.API.Application.DTOs.Requests;
using MonitoraAgro.API.Application.Errors;
using MonitoraAgro.API.Domain.Extensions;

namespace MonitoraAgro.API.Application.UseCases.User.Register;

public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().WithMessage(ErrorMessages.FIRST_NAME_REQUIRED);
        RuleFor(x => x.LastName).NotEmpty().WithMessage(ErrorMessages.LAST_NAME_REQUIRED);
        RuleFor(x => x.Email).NotEmpty().WithMessage(ErrorMessages.EMAIL_REQUIRED);
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(ErrorMessages.PASSWORD_REQUIRED)
            .MinimumLength(8).WithMessage(ErrorMessages.PASSWORD_MIN_LENGTH);
        When(x => x.Email.NotEmpty(), () =>
        {
            RuleFor(x => x.Email).EmailAddress().WithMessage(ErrorMessages.INVALID_EMAIL);
        });
    }
}
